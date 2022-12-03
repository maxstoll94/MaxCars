using Helpers;
using MediatR;
using PersistenceLayer;

namespace DomainLayer.Handlers
{
    public class BookCarMessage : IRequest<Result<Helpers.Unit, DomainError>>
    {
        public string Code { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public BookCarMessage(string code, DateTime start, DateTime end)
        {
            Code = code;
            Start = start;
            End = end;
        }
    }

    public class BookCarHandler : RequestHandler<BookCarMessage, Result<Helpers.Unit, DomainError>>
    {

        private readonly IRepository _repository;

        public BookCarHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override Result<Helpers.Unit, DomainError> Handle(BookCarMessage request)
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


            var bookingDto = new BookingDto(car.Id, request.Start, request.End);
            
            _repository.AddBooking(bookingDto);
            _repository.SaveChanges();

            return Result.Succeed();
        }
    }
}
