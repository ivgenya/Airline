using AuthService.DTO;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController:Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    [HttpGet("{username}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetUserByIdAsync(String username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    
    [HttpGet("GetUserRole")]
    public async Task<IActionResult> GetUserRoleAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Ok("unauthorized");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = roles.First();
        return Ok(userRole);
    }
    
    [HttpGet("all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var adminUsers = await _userManager.GetUsersInRoleAsync("admin");
        var dispatcherUsers = await _userManager.GetUsersInRoleAsync("dispatcher");
        var allUsers = adminUsers.Concat(dispatcherUsers);
        return Ok(allUsers);
    }
    
    [HttpPost("add")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddUser([FromBody] NewUserModel newUserModel)
    {
        var newUser = new ApplicationUser
        {
            UserName = newUserModel.UserName,
            Email = newUserModel.Email
        };
        var result = await _userManager.CreateAsync(newUser, newUserModel.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, newUserModel.Role);
            return Ok("User added as " + newUserModel.Role);
        }
        return BadRequest(result.Errors);
    }
    
    [HttpPut("{username}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdatedUserModel updateUserModel)
    {
        Console.WriteLine(username);
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        user.UserName = updateUserModel.UserName;
        user.Email = updateUserModel.Email;
        if (!string.IsNullOrEmpty(updateUserModel.Role))
        {
            if (await _roleManager.RoleExistsAsync(updateUserModel.Role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, updateUserModel.Role);
            }
            else
            {
                return BadRequest("Роль не существует");
            }
        }
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok("Пользователь успешно обновлен");
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{username}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok("User deleted successfully");
        }
        return BadRequest(result.Errors);
    }


}