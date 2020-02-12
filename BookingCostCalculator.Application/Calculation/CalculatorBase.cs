using System;
using BookingCostCalculator.Domain;

namespace BookingCostCalculator.Application.Calculation
{
    public abstract class CalculatorBase : ICalculator
    {
        public decimal Rate { get; internal set; }
        public abstract bool IsApplicable(Booking booking);
        
        public decimal Calculate(Booking booking)
        {
            if (!IsApplicable(booking))
            {
                return 0m;
            }
            
            var timespan = booking.To.Subtract(booking.From);
            var cost = Convert.ToDecimal(timespan.TotalHours) * Rate;
            return Math.Round(cost, 2, MidpointRounding.ToZero);
        }
    }
}