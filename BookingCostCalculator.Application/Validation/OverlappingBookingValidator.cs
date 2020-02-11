using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class OverlappingBookingValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            return booking.From.Offset == booking.To.Offset;
        }
    }
}