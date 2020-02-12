using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BookingCostCalculator.Application;
using BookingCostCalculator.Application.Calculation;
using BookingCostCalculator.Application.Queries;
using BookingCostCalculator.Application.Validation;
using BookingCostCalculator.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookingCostCalculator.CommandLine
{
    class Program
    {
        public static async Task Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var program = new Program();
            await program.Run(serviceCollection.BuildServiceProvider());
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<BookingRates>(configuration.GetSection("BookingRates"));

            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
            });

            services.AddMediatR(typeof(GetBookingCostsQuery).Assembly);

            //Register validators
            services.AddScoped<IValidator, MinimumBookingValidator>();
            services.AddScoped<IValidator, MaximumBookingValidator>();
            services.AddScoped<IValidator, BookingOffsetValidator>();
            services.AddScoped<IValidator, BookingIncrementValidator>();
            
            //Register calculators
            services.AddScoped<ICalculator, SundayRateCalculator>();
            services.AddScoped<ICalculator, SaturdayRateCalculator>();
            services.AddScoped<ICalculator, NightRateCalculator>();
            services.AddScoped<ICalculator, DayRateCalculator>();
        }

        private async Task Run(IServiceProvider serviceProvider)
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var bookings = ReadBookings();
            var query = new GetBookingCostsQuery
            {
                Bookings = bookings
            };

            var response = await mediator.Send(query);
            WriteBookings(response);
        }
        
        private IEnumerable<Booking> ReadBookings()
        {
            using var stream = typeof(Program).Assembly.GetManifestResourceStream("BookingCostCalculator.CommandLine.Bookings.json");
            using var streamReader = new StreamReader(stream);
            var fileContents = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<IEnumerable<Booking>>(fileContents);
        }

        private void WriteBookings(IEnumerable<Booking> bookings)
        {
            var json = JsonConvert.SerializeObject(bookings, Formatting.Indented);
            File.WriteAllText("output.json", json);
        }
    }
}