namespace PersistenceLayer
{
    public class FakeRepository : IRepository
    {
        private readonly CarDto[] _cars = new CarDto[]
        {
            new CarDto(1, "BMW", 50),
            new CarDto(2, "Merceded", 100),
            new CarDto(3, "Toyota", 20)
        };

        private readonly List<BookingDto> _bookings = new();

        public void AddBooking(BookingDto booking)
        {
            _bookings.Add(booking);
        }

        public IQueryable<BookingDto> GetBookings()
        {
            return _bookings.AsQueryable();
        }

        public CarDto? GetCarByCode(string code)
        {
            return _cars.FirstOrDefault(c => c.Code == code);
        }
    }
}
