using AirlineService.Models;

namespace AirlineService.State;

public class TicketUnpaidState : ITicketState
{
    public static string StatusString => "unpaid";
    
    public TicketUnpaidState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
    }

    public void Pay(Ticket ticket)
    {
        ticket.State = new TicketPaidState(ticket);
    }
    
    public void Expire(Ticket ticket)
    {
        ticket.State = new TicketExpiredState(ticket);
    }

    public void Cancel(Ticket ticket)
    {
        ticket.State = new TicketCancelledState(ticket);
    }

    public void Use(Ticket ticket)
    {
        ticket.State = new TicketUsedState(ticket);
    }

    public void Annul(Ticket ticket)
    {
        ticket.State = new TicketAnnuledState(ticket);
    }
}

public class TicketPaidState : ITicketState
{
    public static string StatusString => "paid";
    
    public TicketPaidState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Paid to Unpaid.");
    }

    public void Pay(Ticket ticket)
    {
    }
    
    public void Expire(Ticket ticket)
    {
        ticket.State = new TicketExpiredState(ticket);
    }

    public void Cancel(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Paid to Cancelled.");
    }

    public void Use(Ticket ticket)
    {
        ticket.State = new TicketUsedState(ticket);
    }

    public void Annul(Ticket ticket)
    {
        ticket.State = new TicketAnnuledState(ticket);
    }
}

public class TicketExpiredState : ITicketState
{
    public static string StatusString => "expired";
    
    public TicketExpiredState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Expired to Unpaid.");
    }

    public void Pay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Expired to Paid.");
    }
    
    public void Expire(Ticket ticket)
    {
        
    }

    public void Cancel(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Expired to Cancelled.");
    }

    public void Use(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Expired to Used.");
    }

    public void Annul(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Expired to Annul.");
    }
}

public class TicketUsedState : ITicketState
{
    public static string StatusString => "used";
    
    public TicketUsedState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Used to Unpaid.");
    }

    public void Pay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Used to Paid.");
    }
    
    public void Expire(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Used to Expired.");
    }

    public void Cancel(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Used to Cancelled.");
    }

    public void Use(Ticket ticket)
    {
    }

    public void Annul(Ticket ticket)
    {
        ticket.State = new TicketAnnuledState(ticket);
    }
}
public class TicketAnnuledState : ITicketState
{
    public static string StatusString => "annuled";
    
    public TicketAnnuledState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Annuled to Unpaid.");
    }

    public void Pay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Annuled to Paid.");
    }
    
    public void Expire(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Annuled to Expired.");
    }

    public void Cancel(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Annuled to Cancelled.");
    }

    public void Use(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Annuled to Used.");
    }

    public void Annul(Ticket ticket)
    {
    }
}

public class TicketCancelledState : ITicketState
{
    public static string StatusString => "cancelled";
    
    public TicketCancelledState(Ticket ticket) {
        ticket.Status = StatusString;
    }

    public void Unpay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Cancelled to Unpaid.");
    }

    public void Pay(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Cancelled to Paid.");
    }
    
    public void Expire(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Cancelled to Expired.");
    }

    public void Cancel(Ticket ticket)
    {
    }

    public void Use(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Cancelled to Used.");
    }

    public void Annul(Ticket ticket)
    {
        throw new InvalidOperationException("Cannot transition from Cancelled to Annul.");
    }
}