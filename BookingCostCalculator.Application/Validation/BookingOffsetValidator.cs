using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public class BookingOffsetValidator : IValidator
    {
        public bool Validate(Booking booking)
        {
            return booking.From.Offset == booking.To.Offset;
        }
    }
}