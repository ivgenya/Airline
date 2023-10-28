using Newtonsoft.Json;

namespace AuthService.Models;

public class UserTicket
{
    public int Id { get; set; }
    public string UserId { get; set; }
    
    [JsonIgnore]
    public ApplicationUser User { get; set; }
}