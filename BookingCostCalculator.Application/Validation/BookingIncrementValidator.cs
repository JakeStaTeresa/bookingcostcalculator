using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class BookingIncrementValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            return booking.To.Subtract(booking.From).TotalMinutes % 15 == 0;
        }
    }
}