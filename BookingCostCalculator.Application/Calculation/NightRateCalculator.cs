using System;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;

namespace BookingCostCalculator.Application.Calculation
{
    public class NightRateCalculator : CalculatorBase
    {
        public NightRateCalculator(IOptions<BookingRates> options)
        {
            Rate = options.Value.Night;
        }
        
        public override bool IsApplicable(Booking booking)
        {
            return InRange(booking.From) || InRange(booking.To);
        }
        
        private bool InRange(DateTimeOffset time)
        {
            return (time.Hour > 20 && time.Hour <= 23) || (time.Hour >= 0 && time.Hour <= 6);
        }
    }
}