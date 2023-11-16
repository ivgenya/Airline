using AirlineService.Models;
using AirlineService.Repositories;
using Quartz;
using Microsoft.Extensions.Logging;

namespace AirlineService.Services;
[DisallowConcurrentExecution]
public class CheckTicketsAndBookingsState : IJob
{
    private readonly ILogger<CheckTicketsAndBookingsState> _logger;
    private readonly IFlightRepository _flightRepository;
    private readonly ITicketRepository _ticketRepository;

    public CheckTicketsAndBookingsState(ILogger<CheckTicketsAndBookingsState> logger, IFlightRepository flightRepository, ITicketRepository ticketRepository)
    {
        _logger = logger;
        _flightRepository = flightRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var flightsToCheck = await _flightRepository.GetFlightsToCheck();
            foreach (var flight in flightsToCheck)
            {
                var tickets = await _ticketRepository.GetAllTicketsByFlightIdAsync(flight.Id);
                foreach (var ticket in tickets)
                {
                    if (ticket.Status.Equals("paid"))
                    {
                        ticket.State.Expire(ticket);
                        await _ticketRepository.UpdateTicketAsync(ticket);
                        var booking = await _ticketRepository.GetBookingByIdAsync(ticket.Id);
                        if (booking != null)
                        {
                            booking.State.Complete(booking);
                            await _ticketRepository.UpdateBookingAsync(booking);
                        }
                    }
                }
            }

            _logger.LogInformation("[INFO] {Now} Booking's and ticket's states updated", DateTime.Now);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogInformation("[ERROR] {Now} An error occured while executing update", DateTime.Now);
        }
    }
}