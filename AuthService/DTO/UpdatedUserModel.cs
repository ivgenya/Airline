using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace AuthService.Models;

public class UpdatedUserModel
{
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
}
