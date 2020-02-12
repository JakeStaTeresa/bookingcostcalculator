using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Application.Queries;
using BookingCostCalculator.Application.Validation;
using BookingCostCalculator.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookingCostCalculator.Application.Tests.Queries
{
    public class GetBookingCostsQueryTests
    {
        private readonly Mock<ILogger<GetBookingCostsQuery.GetBookingCostsQueryHandler>> logger;

        public GetBookingCostsQueryTests()
        {
            logger = new Mock<ILogger<GetBookingCostsQuery.GetBookingCostsQueryHandler>>(MockBehavior.Loose);
        }

        [Fact]
        public async void ValidationFailsWhenOneValidatorFails()
        {
            var mockValidator1 = new Mock<IValidator>();
            mockValidator1.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);
            
            var mockValidator2 = new Mock<IValidator>();
            mockValidator2.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(false);
            
            var validators = new List<IValidator>
            {
                mockValidator1.Object,
                mockValidator2.Object
            };
            
            var queryHandler = new GetBookingCostsQuery.GetBookingCostsQueryHandler(logger.Object, validators, new List<ICalculator>());
            var response = await queryHandler.Handle(new GetBookingCostsQuery
                {
                    Bookings = new []
                    {
                        new Booking
                        {
                            Id = 1,
                        }
                    }
                }, CancellationToken.None);

            var result = response.ToList();

            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);

            var calculatedBooking = result[0];
            Assert.False(calculatedBooking.IsValid);
            Assert.Equal(0m, calculatedBooking.Cost);

            logger.VerifyAll();
            mockValidator1.VerifyAll();
            mockValidator2.VerifyAll();
        }
        
        [Fact]
        public async void ValidationSucceedsWhenAllValidatorsPass()
        {
            var mockValidator1 = new Mock<IValidator>();
            mockValidator1.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);
            
            var mockValidator2 = new Mock<IValidator>();
            mockValidator2.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);
            
            var validators = new List<IValidator>
            {
                mockValidator1.Object,
                mockValidator2.Object
            };
            
            var queryHandler = new GetBookingCostsQuery.GetBookingCostsQueryHandler(logger.Object, validators, new List<ICalculator>());
            var response = await queryHandler.Handle(new GetBookingCostsQuery
            {
                Bookings = new []
                {
                    new Booking
                    {
                        Id = 1,
                    }
                }
            }, CancellationToken.None);

            var result = response.ToList();

            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);

            var calculatedBooking = result[0];
            Assert.True(calculatedBooking.IsValid);
            Assert.Equal(0m, calculatedBooking.Cost);

            logger.VerifyAll();
            mockValidator1.VerifyAll();
            mockValidator2.VerifyAll();
        }
        
        [Fact]
        public async void CalculationDoesNotKickInWhenThereAreNoApplicableCalculators()
        {
            var mockValidator1 = new Mock<IValidator>();
            mockValidator1.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);

            var mockValidator2 = new Mock<IValidator>();
            mockValidator2.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);
            
            var validators = new List<IValidator>
            {
                mockValidator1.Object,
                mockValidator2.Object
            };
            
            var calculator1 = new Mock<ICalculator>();
            calculator1.Setup(v => v.IsApplicable(It.IsAny<Booking>()))
                .Returns(false);
            
            var calculators = new List<ICalculator>
            {
                calculator1.Object,
            };

            var queryHandler = new GetBookingCostsQuery.GetBookingCostsQueryHandler(logger.Object, validators, calculators);
            var response = await queryHandler.Handle(new GetBookingCostsQuery
            {
                Bookings = new []
                {
                    new Booking
                    {
                        Id = 1,
                    }
                }
            }, CancellationToken.None);

            var result = response.ToList();

            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);

            var calculatedBooking = result[0];
            Assert.True(calculatedBooking.IsValid);
            Assert.Equal(0m, calculatedBooking.Cost);

            logger.VerifyAll();
            mockValidator1.VerifyAll();
            mockValidator2.VerifyAll();
        }
        
        [Fact]
        public async void CalculationIsDoneUsingTheFirstApplicableCalculator()
        {
            var mockValidator1 = new Mock<IValidator>();
            mockValidator1.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);

            var mockValidator2 = new Mock<IValidator>();
            mockValidator2.Setup(v => v.Validate(It.IsAny<Booking>()))
                .Returns(true);
            
            var validators = new List<IValidator>
            {
                mockValidator1.Object,
                mockValidator2.Object
            };
            
            var calculator1 = new Mock<ICalculator>();
            calculator1.Setup(v => v.IsApplicable(It.IsAny<Booking>()))
                .Returns(false);
            
            var calculator2 = new Mock<ICalculator>();
            calculator2.Setup(v => v.IsApplicable(It.IsAny<Booking>()))
                .Returns(true);
            calculator2.Setup(v => v.Calculate(It.IsAny<Booking>()))
                .Returns(101m);

            var calculator3 = new Mock<ICalculator>();
            calculator3.Setup(v => v.IsApplicable(It.IsAny<Booking>()))
                .Returns(true);
            calculator3.Setup(v => v.Calculate(It.IsAny<Booking>()))
                .Returns(102m);

            var calculators = new List<ICalculator>
            {
                calculator1.Object,
                calculator2.Object,
                calculator3.Object
            };

            var queryHandler = new GetBookingCostsQuery.GetBookingCostsQueryHandler(logger.Object, validators, calculators);
            var response = await queryHandler.Handle(new GetBookingCostsQuery
            {
                Bookings = new []
                {
                    new Booking
                    {
                        Id = 1,
                    }
                }
            }, CancellationToken.None);

            var result = response.ToList();

            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);

            var calculatedBooking = result[0];
            Assert.True(calculatedBooking.IsValid);
            Assert.Equal(101m, calculatedBooking.Cost);

            logger.VerifyAll();
            mockValidator1.VerifyAll();
            mockValidator2.VerifyAll();
        }
    }
}