using System;
using BookingCostCalculator.Application.Validation;
using BookingCostCalculator.Domain;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Validation
{
    public class BookingOffsetValidatorTests
    {
        private readonly IValidator validator;

        public BookingOffsetValidatorTests()
        {
            validator = new BookingOffsetValidator();
        }
        
        [Fact]
        public void SuccessTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T05:00:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T06:00:00+11:00"),
            };
            
            Assert.True(validator.Validate(booking));
        }
        
        [Fact]
        public void ErrorTests()
        {
            var booking = new Booking
            {
                Id = 1,
                From = DateTimeOffset.Parse("2017-10-23T05:00:00+11:00"),
                To = DateTimeOffset.Parse("2017-10-23T06:00:00-11:00"),
            };
            
            Assert.False(validator.Validate(booking));
        }
    }
}