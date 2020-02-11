using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class MinimumBookingValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            return booking.To.Subtract(booking.From).TotalHours >= 1;
        }
    }
}