using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.Database
{
    public interface IContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        int SaveChanges();
    }
}
