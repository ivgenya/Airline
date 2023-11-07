using AirlineService.Models;

namespace AirlineService.State;

public class BookingConfirmedState : IBookingState
{
    public static string StatusString => "confirmed";
    
    public BookingConfirmedState(Booking booking) {
        booking.Status = StatusString;
    }
    public void Confirm(Booking booking) {
        
    }
    public void Pay(Booking booking) {
        booking.State = new BookingPaidState(booking);
    }

    public void Cancel(Booking booking) {
        booking.State = new BookingCancelledState(booking);
    }

    public void Expire(Booking booking) {
        booking.State = new BookingExpiredState(booking);
    }

    public void Annul(Booking booking) {
        booking.State = new BookingAnnuledState(booking);
    }
}

public class BookingPaidState : IBookingState
{
    public static string StatusString => "paid";
    
    public BookingPaidState(Booking booking) {
        booking.Status = StatusString;
    }
    public void Confirm(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Paid to Confirmed.");
    }
    public void Pay(Booking booking) {

    }

    public void Cancel(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Paid to Cancelled.");
    }

    public void Expire(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Paid to Expired.");
    }

    public void Annul(Booking booking) {
        booking.State = new BookingAnnuledState(booking);
    }
}

public class BookingCancelledState : IBookingState
{
    public static string StatusString => "cancelled";
    
    public BookingCancelledState(Booking booking)
    {
        booking.Status = StatusString;
    }
    public void Confirm(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Cancelled to Confirmed.");
    }
    public void Pay(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Cancelled to Paid.");
    }

    public void Cancel(Booking booking) {
    }

    public void Expire(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Cancelled to Expired.");
    }

    public void Annul(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Cancelled to Annuled.");
    }
}

public class BookingExpiredState : IBookingState
{
    public static string StatusString => "expired";
    
    public BookingExpiredState(Booking booking) {
        booking.Status = StatusString;
    }
    public void Confirm(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Expired to Confirmed.");
    }
    public void Pay(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Expired to Paid.");
    }

    public void Cancel(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Expired to Cancelled.");
    }

    public void Expire(Booking booking) {
        
    }

    public void Annul(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Expired to Annuled.");
    }
}




public class BookingAnnuledState : IBookingState
{
    public static string StatusString => "annuled";
    
    public BookingAnnuledState(Booking booking) {
        booking.Status = StatusString;
    }
    public void Confirm(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Annuled to Confirmed.");
    }
    public void Pay(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Annuled to Paid.");
    }

    public void Cancel(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Annuled to Cancelled.");
    }

    public void Expire(Booking booking) {
        throw new InvalidOperationException("Cannot transition from Annuled to Expired.");
    }
    
    public void Annul(Booking booking) {
    }
}


