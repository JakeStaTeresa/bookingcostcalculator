using System;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Calculation
{
    public class NightRateCalculatorTests
    {
        private readonly NightRateCalculator calculator;

        public NightRateCalculatorTests()
        {
            var options = Options.Create<BookingRates>(new BookingRates
            {
                Night = 42.93m
            });
            
            calculator = new NightRateCalculator(options);
        }

        [Fact]
        public void SuccessTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T21:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-24T05:59:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(374.92m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void FromBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T20:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T23:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(128.77m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void ToBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T23:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-24T06:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(289.77m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void DayTimeTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T06:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T19:59:59+11:00")
            };
            
            Assert.False(calculator.IsApplicable(booking));
            Assert.Equal(0m, calculator.Calculate(booking));
        }
    }
}