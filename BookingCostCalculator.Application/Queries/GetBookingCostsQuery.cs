using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Application.Validation;
using BookingCostCalculator.Domain;
using MediatR;

namespace BookingCostCalculator.Application.Queries
{
    public class GetBookingCostsQuery : IRequest<IEnumerable<Booking>>
    {
        public IEnumerable<Booking> Bookings { get; set; }
        
        public class GetBookingCostsQueryHandler : IRequestHandler<GetBookingCostsQuery, IEnumerable<Booking>>
        {
            private readonly IEnumerable<IValidator> validators;
            private readonly IEnumerable<ICalculator> calculators;
            public GetBookingCostsQueryHandler(IEnumerable<IValidator> validators, IEnumerable<ICalculator> calculators)
            {
                this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
                this.calculators = calculators ?? throw new ArgumentNullException(nameof(calculators));
            }
            
            public Task<IEnumerable<Booking>> Handle(GetBookingCostsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = request.Bookings.Select(b =>
                {
                    //Booking must past all validators
                    var isValid = validators
                        .Select(v => v.Validate(b))
                        .All(r => r);

                    var cost = 0m;
                    if (isValid)
                    {
                        // Booking will perform calculation using the first applicable calculator.
                        // Calculators are in dependency registration order 
                        var calculator = calculators.First(c => c.IsApplicable(b));
                        cost = calculator.Calculate(b);
                    }

                    return new Booking
                    {
                        Id = b.Id,
                        From = b.From,
                        To = b.To,
                        IsValid = isValid,
                        Cost = cost
                    };
                });
                
                return Task.FromResult(returnValue);
            }
        }
    }
}