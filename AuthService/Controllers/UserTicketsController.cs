using System.Net.Http.Headers;
using System.Text;
using AuthService.Data;
using AuthService.DTO;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "client")]
public class UserTicketsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HttpClient _httpClient;
    public UserTicketsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HttpClient httpClient)
    {
        _context = context;
        _userManager = userManager;
        _httpClient = httpClient;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUserTicket(int ticketId)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("Пользователь не авторизован");
            }

            var userId = user.Id;
            var userTicket = new UserTicket()
            {
                Id = ticketId,
                UserId = userId
            };
            _context.userTicket.Add(userTicket);
            _context.SaveChanges();
            return Ok("Данные успешно добавлены");
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка при добавлении данных UserTicket: {ex.Message}");
        }
    }
    
    [HttpGet("GetAllUserTickets")]
    public async Task<IActionResult> GetAllUserTickets()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("Пользователь не авторизован");
            }
            var userId = user.Id;
            var ticIds = await _context.userTicket.Where(user => user.UserId == userId).Select(user => user.Id).ToListAsync();
            var jsonData = JsonConvert.SerializeObject(ticIds);
            string apiUrl = "https://localhost:7175/api/Ticket/GetTicketDetails";
            HttpContext httpContext = HttpContext;
            string authorizationHeader = httpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string bearerToken = authorizationHeader.Substring("Bearer ".Length);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                Console.WriteLine(bearerToken);
            }
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ticketDetails = JsonConvert.DeserializeObject<List<TicketModel>>(responseContent);
                return Ok(ticketDetails);
            }
            return BadRequest("Ошибка при запросе");
        }
        catch (Exception ex)
        {
            return BadRequest($"Произошла ошибка при добавлении данных UserTicket: {ex.Message}");
        }
    }

}