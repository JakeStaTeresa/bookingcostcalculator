using System;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Calculation
{
    public class SundayRateCalculatorTests
    {
        private readonly SundayRateCalculator calculator;

        public SundayRateCalculatorTests()
        {
            var options = Options.Create<BookingRates>(new BookingRates
            {
                Sunday = 60.85m
            });
            
            calculator = new SundayRateCalculator(options);
        }

        [Fact]
        public void SuccessTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-22T06:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-22T20:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(836.68m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void FromBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-22T00:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-22T16:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(973.58m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void ToBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-22T06:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-22T23:59:59+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(1095.26m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void SaturdayOverlapTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-21T21:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-22T06:59:59+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(593.27m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void MondayOverlapTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-22T20:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T06:59:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(653.12m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void NonSundayTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-20T05:00:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-20T21:00:00+11:00")
            };
            
            Assert.False(calculator.IsApplicable(booking));
            Assert.Equal(0m, calculator.Calculate(booking));
        }
    }
}