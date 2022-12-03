using PersistenceLayer.Database;

namespace PersistenceLayer
{
    public class Repository : IRepository
    {
        private readonly IContext _context;

        public Repository(IContext context)
        {
            _context = context;
        }

        public void AddBooking(BookingDto bookingDto)
        {
            var booking = new Booking()
            {
                CarId = bookingDto.CarId,
                End = bookingDto.End,
                Start = bookingDto.Start
            };

            _context.Bookings.Add(booking);
        }

        public IEnumerable<BookingDto> GetBookingsForCar(int carId)
        {
            return _context.Bookings
                .Where(b => b.CarId == carId)
                .Select(b => new BookingDto(b.CarId, b.Start, b.End));
        }

        public CarDto? GetCarByCode(string code)
        {
            return _context.Cars
                .Where(c => c.Code == code)
                .Select(c => new CarDto(c.Id, c.Code, c.Price))
                .FirstOrDefault();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
