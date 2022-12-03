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

        public IEnumerable<BookingDto> GetBookingsForCar(int carId)
        {
            return _bookings.Where(b => b.CarId == carId);
        }

        public CarDto? GetCarByCode(string code)
        {
            return _cars.FirstOrDefault(c => c.Code == code);
        }

        public void SaveChanges() {}
    }
}
