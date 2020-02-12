using System;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Options;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Calculation
{
    public class SaturdayRateCalculatorTests
    {
        private readonly ICalculator calculator;

        public SaturdayRateCalculatorTests()
        {
            var options = Options.Create<BookingRates>(new BookingRates
            {
                Saturday = 45.91m
            });
            
            calculator = new SaturdayRateCalculator(options);
        }

        [Fact]
        public void SuccessTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-21T06:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-21T20:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(631.26m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void FromBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-21T00:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-21T16:00:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(734.54m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void ToBoundaryTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-21T06:00:01+11:00"),
                To = DateTimeOffset.Parse("2017-10-21T23:59:59+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(826.35m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void FridayOverlapTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-20T21:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-21T06:59:59+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(447.60m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void SundayOverlapTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-21T20:15:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-22T06:59:00+11:00")
            };
            
            Assert.True(calculator.IsApplicable(booking));
            Assert.Equal(492.76m, calculator.Calculate(booking));
        }
        
        [Fact]
        public void NonSaturdayTests()
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