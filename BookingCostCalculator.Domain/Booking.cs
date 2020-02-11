using System;

namespace BookingCostCalculator.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public bool IsValid { get; set; }
        public decimal Cost { get; set; }
    }
}