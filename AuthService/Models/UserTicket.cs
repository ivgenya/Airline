namespace AuthService.Models;

public class UserTicket
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}