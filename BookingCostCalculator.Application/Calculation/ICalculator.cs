using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Calculation
{
    public interface ICalculator
    {
        public decimal Rate { get; }
        bool IsApplicable(Booking booking);
        decimal Calculate(Booking booking);
    }
}