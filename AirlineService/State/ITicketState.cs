using AirlineService.Models;

namespace AirlineService.State;

public interface ITicketState
{
    void Unpay(Ticket ticket);
    void Pay(Ticket ticket);
    void Expire(Ticket ticket);
    void Cancel(Ticket ticket);
    void Use(Ticket ticket);
    void Annul(Ticket ticket);
}