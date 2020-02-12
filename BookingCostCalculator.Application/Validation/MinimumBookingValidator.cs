using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class MinimumBookingValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            var totalHours = booking.To.Subtract(booking.From).TotalHours;
            return totalHours >= 1;
        }
    }
}