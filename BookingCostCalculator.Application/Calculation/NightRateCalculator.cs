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
            var isBetween8pmAndMidnight = time > time.Date.AddHours(20).AddMilliseconds(1) && time <= time.Date.AddDays(1);
            var isBetweenMidnightAnd6am = time >= time.Date && time <= time.Date.AddHours(6);
            
            return  isBetween8pmAndMidnight || isBetweenMidnightAnd6am;
        }
    }
}