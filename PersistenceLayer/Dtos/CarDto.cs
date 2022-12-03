namespace PersistenceLayer.Dtos
{
    public class CarDto
    {
        public int Id { get; }
        public string Code { get; }
        public decimal Price { get; }

        public CarDto(int id, string code, decimal price)
        {
            Id = id;
            Code = code;
            Price = price;
        }

    }
}
