using Helpers;
using MediatR;
using PersistenceLayer;

namespace DomainLayer.Handlers
{
    public class CalculateCarPriceMessage : IRequest<Result<CalculateCarPriceResponse, DomainError>>
    {
        public string Code { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public CalculateCarPriceMessage(string code, DateTime start, DateTime end)
        {
            Code = code;
            Start = start;
            End = end;
        }

    }

    public record CalculateCarPriceRates(decimal CarRate, decimal InsuranceRate, decimal MaxCarRate);
    public record CalculateCarPriceResponse(CalculateCarPriceRates Rates, decimal Discount, decimal Total);

    public class CalculateCarPriceHandler : RequestHandler<CalculateCarPriceMessage, Result<CalculateCarPriceResponse, DomainError>>
    {
        private readonly IRepository _repository;

        public CalculateCarPriceHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override Result<CalculateCarPriceResponse, DomainError> Handle(CalculateCarPriceMessage request)
        {
            if (request.End <= request.Start)
            {
                var error = new DomainError(ErrorCode.InvalidDateTime, $"The provided DateTime range is invalid");
                return Result.Fail(error);
            }

            var car = _repository.GetCarByCode(request.Code);

            if (car == null) 
            {
                var error = new DomainError(ErrorCode.CarNotFound, $"A car with code: {request.Code} was not found");
                return Result.Fail(error);
            }

            var bookings = _repository.GetBookingsForCar(car.Id);

            // Verify that the car is NOT booked during the request time period
            if (!bookings.All(booking => request.End < booking.Start || request.Start > booking.End))
            {
                var error = new DomainError(ErrorCode.CarBooked, $"The car with code: {request.Code} is already booked");
                return Result.Fail(error);

            }

            // Calculate the car rate
            (var carRate, var days) = CalculateCarRate(car.Price, request.Start, request.End);

            // Calculate the insurance rate
            var insuranceRate = CalculateInsuranceRate(carRate, days);

            // Calculate the max car rate
            var maxCarRate = CalculateMaxCarRate(carRate, days);

            // Calculate total and discount
            var total = carRate + insuranceRate + maxCarRate;
            var discount = CalculateDiscount(total, days);
            total -= discount;

            var rates = new CalculateCarPriceRates(carRate, insuranceRate, maxCarRate);
            var response = new CalculateCarPriceResponse(rates, discount, total);

            return Result.Succeed(response);
        }


        /// <summary>
        /// On Saturday and Sunday the base price of the car goes up with 5%.
        /// </summary>
        public static (decimal CarRate, int Days) CalculateCarRate(decimal carRate, DateTime start, DateTime end)
        {
            var weekendRate = 0m;
            var days = 0;
            var perc = (0.05m * carRate);

            foreach (var day in EachDay(start, end))
            {
                if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendRate += perc;
                }

                days++;
            }

            carRate += weekendRate;

            return (carRate * days, days);
        }

        /// <summary>
        /// Insurance adds 10% per day on top of the car price.
        /// </summary>
        public static decimal CalculateInsuranceRate(decimal carRate, int days)
        {
            return (0.1m * carRate) * days;
        }

        /// <summary>
        /// Snappcar adds 10% per day on top of the car price
        /// </summary>
        public static decimal CalculateMaxCarRate(decimal carRate, int days)
        {
            return (0.1m * carRate) * days;
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public static decimal CalculateDiscount(decimal total, int days)
        {
            var discount = 0m;

            if (days > 3)
            {
                discount = 0.15m * total; 
            }

            return discount;
        }
    }
}
