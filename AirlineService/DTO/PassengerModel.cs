using System.ComponentModel.DataAnnotations;

namespace AirlineService.DTO;

public class PassengerModel
{
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string DocumentNumber { get; set; } = null!;
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string? Gender { get; set; }
    [Required]
    public string Email { get; set; } = null!;
}