using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Application.Validation;
using BookingCostCalculator.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingCostCalculator.Application.Queries
{
    public class GetBookingCostsQuery : IRequest<IEnumerable<Booking>>
    {
        public IEnumerable<Booking> Bookings { get; set; }
        
        public class GetBookingCostsQueryHandler : IRequestHandler<GetBookingCostsQuery, IEnumerable<Booking>>
        {
            private readonly ILogger<GetBookingCostsQueryHandler> logger;
            private readonly IEnumerable<IValidator> validators;
            private readonly IEnumerable<ICalculator> calculators;
            public GetBookingCostsQueryHandler(ILogger<GetBookingCostsQueryHandler> logger, IEnumerable<IValidator> validators, IEnumerable<ICalculator> calculators)
            {
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
                this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
                this.calculators = calculators ?? throw new ArgumentNullException(nameof(calculators));
                
                logger.LogInformation("Starting GetBookingCostsQueryHandler");
                logger.LogInformation("Validators will be executed in the following order: ");
                foreach (var validator in validators)
                {
                    logger.LogInformation($" * {validator.GetType().Name}");
                }
                
                logger.LogInformation("Calculators will be evaluated in the following order: ");
                foreach (var calculator in calculators)
                {
                    logger.LogInformation($" * {calculator.GetType().Name}");
                }
            }
            
            public async Task<IEnumerable<Booking>> Handle(GetBookingCostsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = request.Bookings.Select(CalculateCost);
                return returnValue;
            }

            private Booking CalculateCost(Booking booking)
            {
                var isValid = validators
                    .Select(v => v.Validate(booking))
                    .All(r => r);

                var cost = 0m;
                if (isValid)
                {
                    // Calculator with highest applicable rate is applied. This is dictated
                    // by dependency registration order
                    var calculator = calculators.FirstOrDefault(c => c.IsApplicable(booking));
                    if (calculator != null)
                    {
                        cost = calculator.Calculate(booking);
                    }
                }

                return new Booking
                {
                    Id = booking.Id,
                    From = booking.From,
                    To = booking.To,
                    IsValid = isValid,
                    Cost = cost
                };
            }
        }
    }
}