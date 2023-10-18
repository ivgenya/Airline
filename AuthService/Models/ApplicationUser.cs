using AirlineService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Models;

public class ApplicationUser: IdentityUser
{
    public ICollection<UserTicket> tickets { get; set; }
}