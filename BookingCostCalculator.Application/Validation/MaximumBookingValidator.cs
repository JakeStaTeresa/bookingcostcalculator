using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class MaximumBookingValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            var totalHours = booking.To.Subtract(booking.From).TotalHours;
            return 0 < totalHours && totalHours <= 24;
        }
    }
}