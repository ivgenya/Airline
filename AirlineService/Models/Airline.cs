namespace AirlineService.Models
{
    public partial class Airline
    {
        public Airline()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
