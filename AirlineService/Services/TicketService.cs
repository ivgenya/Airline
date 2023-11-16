using AirlineService.Data;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Repositories;
using Microsoft.EntityFrameworkCore;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AirlineService.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IFlightRepository _flightRepository;

    public TicketService(ITicketRepository ticketRepository, IFlightRepository flightRepository)
    {
        _ticketRepository = ticketRepository;
        _flightRepository = flightRepository;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _ticketRepository.GetAllTicketsAsync();
    }

    public async Task<Ticket> GetTicketByIdAsync(int ticketId)
    {
        return await _ticketRepository.GetTicketByIdAsync(ticketId);
    }
    public async Task<Ticket> GetTicketByCodeAsync(string? code)
    {
        return await _ticketRepository.GetTicketByCodeAsync(code);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByBookingIdAsync(int bookingId)
    {
        return await _ticketRepository.GetTicketsByBookingIdAsync(bookingId);
    }

    public async Task<int> CreateTicketAsync(Ticket ticket)
    {
        return await _ticketRepository.CreateTicketAsync(ticket);
    }

    public async Task<int> CreatePassengerAsync(Passenger passenger)
    {
        return await _ticketRepository.CreatePassengerAsync(passenger);
    }


    public async Task<int> UpdateTicketAsync(Ticket ticket)
    {
        return await _ticketRepository.UpdateTicketAsync(ticket);
    }

    public async Task<int> DeleteTicketAsync(int ticketId)
    {
        return await _ticketRepository.DeleteTicketAsync(ticketId);
    }


    public async Task<int> BuyTicketAsync(Passenger passenger, int flightId, int seatId)
    {
        int id = await _ticketRepository.CreatePassengerAsync(passenger);
        var _passenger = await _ticketRepository.GetPassengerByIdAsync(id);
        var flight = await _flightRepository.GetFlightByIdAsync(flightId);
        var seat = await _ticketRepository.GetSeatByIdAsync(seatId);
        if (flight != null && seat != null && seat.Status.Equals("available"))
        {
            var ticket = new Ticket
            {
                PassengerId = _passenger.Id,
                FlightId = flightId,
                BookingId = null,
                SeatId = seatId,
                DateOfPurchase = DateTime.Now,
                BaggageType = "economy"
            };
            var ticketId = await _ticketRepository.CreateTicketAsync(ticket);
            StartPaymentTimer(ticketId);
            return ticketId;
        }
        return -1;
    }

    public async Task<int> ReserveTicketAsync(int ticketId)
    {
        var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
        if (ticket != null && ticket.Status == "unpaid")
        {
            var booking = new Booking
            {
                BookingDate = DateTime.Now,
            };
            booking.Tickets.Add(ticket);
            ticket.Booking = booking;

            var bookingId = await _ticketRepository.CreateBookingAsync(booking);
            await _ticketRepository.UpdateTicketAsync(ticket);
            StartBookingTimer(booking.Id);
            return bookingId;
        }
        return -1;
    }

    public async Task<Booking> GetBookingByIdAsync(int? bookingId)
    {
        return await _ticketRepository.GetBookingByIdAsync(bookingId);
    }
    
    public async Task<Booking> GetBookingByCodeAsync(string? code)
    {
        return await _ticketRepository.GetBookingByCodeAsync(code);
    }
    public async Task<int> UpdateBookingAsync(Booking booking)
    {
        return await _ticketRepository.UpdateBookingAsync(booking);
    }
    
    public async Task<int> UpdateSeatAsync(Seat seat)
    {
        return await _ticketRepository.UpdateSeatAsync(seat);
    }
    private void StartBookingTimer(int bookingId)
    {
        int timerInterval = 60 * 60 * 1000;
        Timer bookingTimer = new Timer(async state => await CancelBookingCallback((int)state), bookingId, 
            (long)timerInterval, Timeout.Infinite);
    }
    
    private void StartPaymentTimer(int ticketId)
    {
        int timerInterval = 60 * 10 * 1000;
        Timer paymentTimer = new Timer(async state => await CancelPaymentCallback((int)state), ticketId, 
            (long)timerInterval, Timeout.Infinite);
    }
    private async Task CancelPaymentCallback(int ticketId)
    {
        using (var context = new AirlineDbContext())
        {
            var ticket = await context.Tickets.FindAsync(ticketId);
            if (ticket != null && ticket.Status.Equals("unpaid") && ticket.BookingId == null)
            {
                var seat = await context.Seats.FindAsync(ticket.SeatId);
                seat.Status = "available";
                context.Entry(seat).State = EntityState.Modified;
                await context.SaveChangesAsync();
                context.Tickets.Remove(ticket);
                await context.SaveChangesAsync();
            }
        }
    }

    public byte[] GenerateBoardingPass(BoardingPassModel model)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (FileStream templateStream = new FileStream("Resources/template_bp.pdf", FileMode.Open))
            {
                PdfReader pdfReader = new PdfReader(templateStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream);

                AcroFields form = pdfStamper.AcroFields;
                form.SetField("passenger", model.Surname.ToUpper() + " " + model.Name.ToUpper());
                form.SetField("from", model.DepShortName);
                form.SetField("to", model.ArrShortName);
                form.SetField("date", model.Date.ToShortDateString());
                form.SetField("dep_time", model.DepartureTime.ToString());
                form.SetField("arr_time", model.ArrivalTime.ToString());
                form.SetField("flight", model.ShortName + model.Number);
                form.SetField("seat", model.Seat);
                form.SetField("gate", model.Gate.ToString());
                form.SetField("from_small", model.DepShortName);
                form.SetField("to_small", model.ArrShortName);
                form.SetField("date_small", model.Date.ToShortDateString() + " " + model.DepartureTime.ToString());
                form.SetField("flight_small", model.ShortName + model.Number);
                form.SetField("seat_small", model.Seat);
                form.SetField("gate_small", model.Gate.ToString());
                pdfStamper.FormFlattening = true; 

                pdfStamper.Close();
                pdfReader.Close();
            }
            return memoryStream.ToArray();
        }
    }
    
    public byte[] GenerateTicket(BoardingPassModel model)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (FileStream templateStream = new FileStream("Resources/template_ticket.pdf", FileMode.Open))
            {
                PdfReader pdfReader = new PdfReader(templateStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream);

                AcroFields form = pdfStamper.AcroFields;
                form.SetField("ticket_number", model.TicketCode);
                if (model.BookingCode != null)
                {
                    form.SetField("booking_number", model.BookingCode);
                }
                else
                {
                    form.SetField("booking_number", "-");
                }
                form.SetField("date", model.DateOfPurchase.ToShortDateString());
                form.SetField("surname", model.Surname.ToUpper());
                form.SetField("name", model.Name.ToUpper());
                form.SetField("document", model.DocumentNumber);
                form.SetField("date_of_birth", model.DateOfBirth.ToShortDateString());
                form.SetField("email", model.Email);
                form.SetField("number", model.ShortName + model.Number);
                form.SetField("dep_short_name", model.DepShortName);
                form.SetField("arr_short_name", model.ArrShortName);
                form.SetField("flight_date", model.Date.ToShortDateString());
                form.SetField("dep_time", model.DepartureTime.ToString());
                form.SetField("arr_time", model.ArrivalTime.ToString());
                form.SetField("dep_city", model.DepCity);
                form.SetField("arr_city", model.ArrCity);
                form.SetField("duration", "В пути: "+model.FlightDuration.ToString());
                form.SetField("class", model.Class);
                form.SetField("baggage", model.BaggageType);
                form.SetField("price", model.Price.ToString() + "p.");
                form.SetField("total_price", model.Price.ToString() + "p.");
                form.SetField("seat", model.Seat);
                form.SetField("terminal", model.Terminal);
                pdfStamper.FormFlattening = true; 

                pdfStamper.Close();
                pdfReader.Close();
            }
            return memoryStream.ToArray();
        }
    }
    
    private async Task CancelBookingCallback(int bookingId)
    {
        try
        {
            using (var context = new AirlineDbContext())
            {
                var booking = await context.Bookings.FindAsync(bookingId);
                if (booking != null)
                {
                    var tickets = await context.Tickets.Where(t => t.BookingId == bookingId).ToListAsync();
                    if (booking.Status != "paid")
                    {
                        foreach (var ticket in tickets)
                        {
                            var seat = await context.Seats.FindAsync(ticket.SeatId);
                            seat.Status = "available";
                            context.Entry(seat).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                            ticket.State.UnableToPay(ticket);
                            context.Entry(ticket).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                        }
                    }
                    booking.State.Expire(booking);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<bool> MakePaymentAsync(int ticketId, PaymentModel paymentInfo)
    {
        try
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket.BookingId != null)
            {
                var booking = await _ticketRepository.GetBookingByIdAsync(ticket.BookingId);
                booking.State.Pay(booking);
                await _ticketRepository.UpdateBookingAsync(booking);
            }

            ticket.State.Pay(ticket);
            ticket.DateOfPurchase = DateTime.Now;
            await _ticketRepository.UpdateTicketAsync(ticket);
            return true;
        }
        catch (InvalidOperationException ex)
        {
            return false;
        }
    }
    
    public async Task<BoardingPassModel?> GetBoardingPassAsync(int ticketId)
    {
        return await _ticketRepository.GetBoardingPassAsync(ticketId);
    }

    public async Task<Seat> GetSeatByIdAsync(int ticketId)
    {
        return await _ticketRepository.GetSeatByIdAsync(ticketId);
    }
    
    public async Task<IEnumerable<Seat>> GetSeatByFlightIdAsync(int flightId)
    {
        var seats = await _ticketRepository.GetSeatByFlightIdAsync(flightId);
        var availableSeats = seats.Where(seat => seat.Status.Equals("available"));
        return availableSeats;
    }
}