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
            return await _ticketRepository.CreateTicketAsync(ticket);
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
        int timerInterval = 60 * 1000;
        Timer bookingTimer = new Timer(async state => await CancelBookingCallback((int)state), bookingId, (long)timerInterval, Timeout.Infinite);
    }

    public byte[] GenerateBoardingPass(BoardingPassModel model)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (FileStream templateStream = new FileStream("Helpers/template.pdf", FileMode.Open))
            {
                PdfReader pdfReader = new PdfReader(templateStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream);

                AcroFields form = pdfStamper.AcroFields;
                form.SetField("passenger", model.Surname.ToUpper() + " " + model.Name.ToUpper());
                form.SetField("from", model.DepCity.ToUpper() + " " + model.DepShortName);
                form.SetField("to", model.ArrCity.ToUpper() + " " + model.ArrShortName);
                form.SetField("date", model.Date.ToShortDateString());
                form.SetField("flight", model.ShortName + " " + model.Number);
                form.SetField("time", model.DepartureTime.ToString());
                form.SetField("seat", model.Seat);
                form.SetField("gate", model.Gate.ToString());
                pdfStamper.FormFlattening = true; 

                pdfStamper.Close();
                pdfReader.Close();
            }

            return memoryStream.ToArray();
        }
        
    }
    
    private async Task CancelBookingCallback(int bookingId)
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
                        context.Tickets.Remove(ticket);
                        var seat = await _ticketRepository.GetSeatByIdAsync(ticket.SeatId);
                        seat.Status = "available";
                        await _ticketRepository.UpdateSeatAsync(seat);
                    }
                }

                booking.State.Expire(booking);
                await context.SaveChangesAsync();
            }
        }
    }

    public async Task<bool> MakePaymentAsync(int ticketId, PaymentModel paymentInfo)
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
    
    

    public async Task<BoardingPassModel?> GetBoardingPassAsync(int ticketId)
    {
        return await _ticketRepository.GetBoardingPassAsync(ticketId);
    }

    public async Task<Seat> GetSeatByIdAsync(int ticketId)
    {
        return await _ticketRepository.GetSeatByIdAsync(ticketId);
    }
}