using System.Net.Http.Headers;
using System.Text;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AirlineService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly HttpClient _httpClient;

    public TicketController(ITicketService ticketService, HttpClient httpClient)
    {
        _service = ticketService;
        _httpClient = httpClient;
    }
    
    [HttpPost("buy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> BuyTicket([FromBody]PassengerModel passengerModel, int flightId, int seatId)
    {
        HttpContext httpContext = HttpContext;
        string authorizationHeader = httpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            Console.WriteLine(bearerToken);
        }
        Console.WriteLine("success");
        var seat = await _service.GetSeatByIdAsync(seatId);
        if (seat == null)
        {
            return BadRequest("Место не существует");
        }
        if (seat.Status.Equals("reserved"))
        {
            return BadRequest("Место занято");
        }
        var passenger = new Passenger
        {
            Surname = passengerModel.Surname,
            Name = passengerModel.Name,
            DocumentNumber = passengerModel.DocumentNumber,
            DateOfBirth = passengerModel.DateOfBirth,
            Gender = passengerModel.Gender,
            Email = passengerModel.Email
        };
        var ticketId = await _service.BuyTicketAsync(passenger, flightId, seatId);
        var ticket = await _service.GetTicketByIdAsync(ticketId);
        seat.Status = "reserved";
        await _service.UpdateSeatAsync(seat);

        var jsonData = new StringContent(JsonConvert.SerializeObject(ticketId), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7066/api/UserTickets?ticketId=" + ticketId, jsonData);
        Console.WriteLine("Request:");
        Console.WriteLine(response.RequestMessage);
        Console.WriteLine("Response:");
        Console.WriteLine(response);
        Console.WriteLine(response.Content);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Успешный ответ от authservice:");
            Console.WriteLine(responseContent);
        }
        else
        {
            Console.WriteLine("Ошибка при отправке POST-запроса в ServiceB");
        }
        
        if (ticket == null)
            return BadRequest("An error has occurred");

        return Ok(ticket);
    }
    
    [HttpPost("book/{ticketId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> BookTicket(int ticketId)
    {
        var bookingId = await _service.ReserveTicketAsync(ticketId);
        Console.WriteLine(bookingId);
        if (bookingId != -1)
        {
            return Ok(bookingId);
        }
        return BadRequest("Booking failed");
    }
    
    [HttpPost("pay/{ticketId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> PayForTicket(int ticketId, [FromQuery] PaymentModel paymentInfo)
    {
        var paymentResult = await _service.MakePaymentAsync(ticketId, paymentInfo);

        if (paymentResult)
        {
            return Ok("Payment successful");
        }
        return BadRequest("Payment failed");
    }

    [HttpGet("book/{id}")]
    public async Task<ActionResult<Booking>> GetBookingById(int id)
    {
        var booking = await _service.GetBookingByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        return Ok(booking);
    }
    
    [HttpPost("register")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "client")]
    public async Task<IActionResult> RegisterForFlight(int ticketId)
    {
        try
        {
            var ticket = await _service.GetTicketByIdAsync(ticketId);
            if (ticket == null)
            {
                return NotFound("Билет с указанным номером не найден.");
            }
            var boardingPass = await _service.GetBoardingPassAsync(ticket.Id);
            if (DateTime.Now.Add(new TimeSpan(0, 3, 0, 0)) < DateTime.Today.Add(boardingPass.DepartureTime))
            {
                Console.WriteLine(DateTime.Now.Add(new TimeSpan(0, 3, 0, 0)));
                Console.WriteLine(DateTime.Today.Add(boardingPass.DepartureTime));
                return BadRequest("Регистрация не началась.");
            }
            if (DateTime.Now.Add(new TimeSpan(0, 1, 0, 0)) > DateTime.Today.Add(boardingPass.DepartureTime))
            {
                return BadRequest("Регистрация закончилась.");
            }
            if (ticket.Status == "paid" )
            {
                ticket.State.Use(ticket);
                await _service.UpdateTicketAsync(ticket);
                if (ticket.BookingId != null)
                {
                    var booking = await _service.GetBookingByIdAsync(ticket.BookingId);
                    booking.State.Complete(booking);
                    await _service.UpdateBookingAsync(booking);
                }
                var pdfBytes = _service.GenerateBoardingPass(boardingPass);
                await _service.UpdateTicketAsync(ticket);
                return File(pdfBytes, "application/pdf", "boarding-pass.pdf");
            }
            else
            {
                return BadRequest("Невозможно зарегистрировать билет, так как он не оплачен.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Произошла ошибка при регистрации на рейс: " + ex.Message);
        }
    }
    [HttpPost("cancel/{bookingId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> CancellBoking(int bookingId)
    {
        var booking = await _service.GetBookingByIdAsync(bookingId);
        var tickets = await _service.GetTicketsByBookingIdAsync(bookingId);

        if (booking == null)
        {
            return BadRequest("Не существует бронирования с таким номером");
        }
        if (!booking.Status.Equals("confirmed"))
        {
            return BadRequest("Бронирование нельзя отменить");
        }
        booking.State.Cancel(booking);
        await _service.UpdateBookingAsync(booking);
        foreach (var ticket in tickets)
        {
            await _service.DeleteTicketAsync(ticket.Id);
        }
        return Ok("Бронирование отменено");
    }
    
    [HttpPost("GetTicketDetails")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> GetTicketDetails([FromBody] List<int> ticketIds)
    {
        Console.WriteLine(ticketIds);
        if (ticketIds == null)
        {
            return BadRequest("Некорректные параметры запроса");
        }

        List<TicketModel> ticketDetailsList = new List<TicketModel>();

        foreach (var id in ticketIds)
        {
            var ticket = await _service.GetTicketByIdAsync(id);
            if (ticket != null)
            {
                var ticketDetails = new TicketModel
                {
                    PassengerId = ticket.PassengerId,
                    FlightId = ticket.FlightId,
                    BookingId = ticket.BookingId,
                    SeatId = ticket.SeatId,
                    DateOfPurchase = ticket.DateOfPurchase,
                    Status = ticket.Status,
                    BaggageType = ticket.BaggageType
                };
                ticketDetailsList.Add(ticketDetails);
            }
        }

        return Ok(ticketDetailsList);
    }

}