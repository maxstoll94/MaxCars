using PersistenceLayer.Dtos;

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

        public CarDto? GetCarByCode(string code)
        {
            return _cars.FirstOrDefault(c => c.Code == code);
        }
    }
}
