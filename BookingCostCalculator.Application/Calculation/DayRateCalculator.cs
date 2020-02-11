using System;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;

namespace BookingCostCalculator.Application.Calculation
{
    public class DayRateCalculator: CalculatorBase
    {
        public DayRateCalculator(IOptions<BookingRates> options)
        {
            Rate = options.Value.Day;
        }
        
        public override bool IsApplicable(Booking booking)
        {
            return InRange(booking.From) || InRange(booking.To);
        }
        
        private bool InRange(DateTimeOffset time)
        {
            return time.Hour > 6 && time.Hour <= 20;
        }
    }
}