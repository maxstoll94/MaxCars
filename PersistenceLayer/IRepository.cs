namespace PersistenceLayer
{
    public record CarDto(int Id, string Code, decimal Price);

    public record BookingDto(int CarId, DateTime Start, DateTime End);

    public interface IRepository
    {
        public CarDto? GetCarByCode(string code);
        public IQueryable<BookingDto> GetBookings();
        public void AddBooking(BookingDto booking);
    }
}
