using AirlineService.Models;

namespace AirlineService.State;

public interface IBookingState
{
    void Confirm(Booking booking);
    void Pay(Booking booking);
    void Cancel(Booking booking);
    void Complete(Booking booking);
    void Expire(Booking booking);
    void Annul(Booking booking);
}