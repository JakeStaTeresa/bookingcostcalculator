using System;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;

namespace BookingCostCalculator.Application.Calculation
{
    public class SundayRateCalculator : CalculatorBase
    {
        public SundayRateCalculator(IOptions<BookingRates> options)
        {
            Rate = options.Value.Sunday;
        }
        
        public override bool IsApplicable(Booking booking)
        {
            return booking.From.DayOfWeek == DayOfWeek.Sunday || booking.To.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}