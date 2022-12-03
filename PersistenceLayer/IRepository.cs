using PersistenceLayer.Dtos;

namespace PersistenceLayer
{
    public interface IRepository
    {
        public CarDto? GetCarByCode(string code);
    }
}
