using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Passenger
    {
        public Passenger()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Email { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
