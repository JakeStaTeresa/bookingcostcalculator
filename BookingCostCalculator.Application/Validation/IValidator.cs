using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Validation
{
    public interface IValidator
    {
        bool Validate(Booking booking);
    }
}