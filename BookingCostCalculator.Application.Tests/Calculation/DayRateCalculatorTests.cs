using System;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Calculation
{
    public class DayRateCalculatorTests
    {
        private readonly DayRateCalculator calculator;

        public DayRateCalculatorTests()
        {
            var options = Options.Create<BookingRates>(new BookingRates
            {
                Day = 38m
            });
            
            calculator = new DayRateCalculator(options);
        }

        [Fact]
        public void SuccessTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T06:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T20:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(522.5m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void FromBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T06:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T16:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(370.5m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void ToBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T06:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T20:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(522.5m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void BeforeMidnightTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T20:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T23:59:00+11:00")
            };
            
            Assert.False(calculator.IsApplicable(booking));
            Assert.Equal(0m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void Before6amTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T05:00:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T21:00:00+11:00")
            };
            
            Assert.False(calculator.IsApplicable(booking));
            Assert.Equal(0m, calculator.Calculate(booking));
        }
    }
}