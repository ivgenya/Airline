using AirlineService.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Data
{
    public partial class AirlineDbContext : DbContext
    {
        public AirlineDbContext()
        {
        }

        public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Airline> Airlines { get; set; } = null!;
        public virtual DbSet<Airport> Airports { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Flight> Flights { get; set; } = null!;
        public virtual DbSet<Passenger> Passengers { get; set; } = null!;
        public virtual DbSet<Plane> Planes { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Seat> Seats { get; set; } = null!;
        public virtual DbSet<Terminal> Terminals { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=airline;uid=root;pwd=Kbfe186*", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Airline>(entity =>
            {
                entity.ToTable("airline");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(3)
                    .HasColumnName("short_name");
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.ToTable("airport");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(3)
                    .HasColumnName("short_name")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("booking_date");
                
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('confirmed','paid','completed','cancelled','expired','annulled')")
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("flight");

                entity.HasIndex(e => e.PlaneId, "plane_id");

                entity.HasIndex(e => e.ScheduleId, "schedule_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Gate).HasColumnName("gate");

                entity.Property(e => e.PlaneId).HasColumnName("plane_id");

                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('boarding','cancelled','check_in','on_time','delayed','departed','expected','landed')")
                    .HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasColumnType("enum('regular','charter')")
                    .HasColumnName("type");

                entity.HasOne(d => d.Plane)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.PlaneId)
                    .HasConstraintName("flight_ibfk_2");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("flight_ibfk_1");
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.ToTable("passenger");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(255)
                    .HasColumnName("document_number");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasColumnType("enum('male','female')")
                    .HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Plane>(entity =>
            {
                entity.ToTable("plane");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PlaneName)
                    .HasMaxLength(255)
                    .HasColumnName("plane_name");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.HasIndex(e => e.AirlineId, "airline_id");

                entity.HasIndex(e => e.ArrivalAirportId, "arrival_airport_id");

                entity.HasIndex(e => e.DepartureAirportId, "departure_airport_id");

                entity.HasIndex(e => e.Terminal, "terminal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AirlineId).HasColumnName("airline_id");

                entity.Property(e => e.ArrivalAirportId).HasColumnName("arrival_airport_id");

                entity.Property(e => e.ArrivalTime)
                    .HasColumnType("time")
                    .HasColumnName("arrival_time");

                entity.Property(e => e.DepartureAirportId).HasColumnName("departure_airport_id");

                entity.Property(e => e.DepartureTime)
                    .HasColumnType("time")
                    .HasColumnName("departure_time");

                entity.Property(e => e.FlightDuration)
                    .HasColumnType("time")
                    .HasColumnName("flight_duration");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Terminal).HasColumnName("terminal");

                entity.HasOne(d => d.Airline)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.AirlineId)
                    .HasConstraintName("schedule_ibfk_1");

                entity.HasOne(d => d.ArrivalAirport)
                    .WithMany(p => p.ScheduleArrivalAirports)
                    .HasForeignKey(d => d.ArrivalAirportId)
                    .HasConstraintName("schedule_ibfk_3");

                entity.HasOne(d => d.DepartureAirport)
                    .WithMany(p => p.ScheduleDepartureAirports)
                    .HasForeignKey(d => d.DepartureAirportId)
                    .HasConstraintName("schedule_ibfk_2");

                entity.HasOne(d => d.TerminalNavigation)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.Terminal)
                    .HasConstraintName("schedule_ibfk_4");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("seat");

                entity.HasIndex(e => e.FlightId, "flight_id");

                entity.HasIndex(e => e.PlaneId, "plane_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Class)
                    .HasColumnType("enum('economy','business','first')")
                    .HasColumnName("class");

                entity.Property(e => e.FlightId).HasColumnName("flight_id");

                entity.Property(e => e.Number)
                    .HasMaxLength(4)
                    .HasColumnName("number");

                entity.Property(e => e.PlaneId).HasColumnName("plane_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('available','reserved','annulled')")
                    .HasColumnName("status");

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("seat_ibfk_1");

                entity.HasOne(d => d.Plane)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.PlaneId)
                    .HasConstraintName("seat_ibfk_2");
            });

            modelBuilder.Entity<Terminal>(entity =>
            {
                entity.ToTable("terminal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(1)
                    .HasColumnName("name")
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasColumnType("enum('internal','international')")
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("ticket");

                entity.HasIndex(e => e.BookingId, "booking_id");

                entity.HasIndex(e => e.FlightId, "flight_id");

                entity.HasIndex(e => e.PassengerId, "passenger_id");

                entity.HasIndex(e => e.SeatId, "seat_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaggageType)
                    .HasColumnType("enum('economy','business','first')")
                    .HasColumnName("baggage_type");
                
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.DateOfPurchase)
                    .HasColumnType("datetime")
                    .HasColumnName("date_of_purchase");

                entity.Property(e => e.FlightId).HasColumnName("flight_id");

                entity.Property(e => e.PassengerId).HasColumnName("passenger_id");

                entity.Property(e => e.SeatId).HasColumnName("seat_id");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('paid','unpaid','unabled to pay','used','expired','cancelled','annulled')")
                    .HasColumnName("status");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ticket_ibfk_3");

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("ticket_ibfk_2");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("ticket_ibfk_1");

                entity.HasOne(d => d.Seat)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("ticket_ibfk_4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
