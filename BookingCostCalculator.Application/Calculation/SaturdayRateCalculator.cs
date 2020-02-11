using System;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;

namespace BookingCostCalculator.Application.Calculation
{
    public class SaturdayRateCalculator: CalculatorBase
    {
        public SaturdayRateCalculator(IOptions<BookingRates> options)
        {
            Rate = options.Value.Saturday;
        }
        
        public override bool IsApplicable(Booking booking)
        {
            return booking.From.DayOfWeek == DayOfWeek.Saturday || booking.To.DayOfWeek == DayOfWeek.Saturday;
        }
    }
}