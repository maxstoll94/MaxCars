using System.ComponentModel.DataAnnotations.Schema;

namespace PersistenceLayer.Database
{
    [Table("Car")]
    public class Car
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
    }

    [Table("Booking")]
    public class Booking
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual Car Car { get; set; }
    }

}
