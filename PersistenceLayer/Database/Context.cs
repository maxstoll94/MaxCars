using Microsoft.EntityFrameworkCore;

namespace PersistenceLayer.Database
{
    public class Context : DbContext, IContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public Context(DbContextOptions<Context> options) :
            base(options)
        {

        }
    }
}
