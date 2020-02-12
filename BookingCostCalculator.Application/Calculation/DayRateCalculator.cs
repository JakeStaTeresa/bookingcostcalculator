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
            var isFromAfter6am = booking.From.Date.AddHours(6).AddMilliseconds(1) <= booking.From;
            var isNotOverlapping = booking.From <= booking.To;
            var isToBefore8pm = booking.To <= booking.From.Date.AddHours(20);
            
            return  isFromAfter6am &&
                    isNotOverlapping &&
                    isToBefore8pm;
        }
    }
}