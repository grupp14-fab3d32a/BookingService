using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class BookingContext(DbContextOptions<BookingContext> options) : DbContext(options)
{
    public DbSet<Entities.BookingEntity> Bookings { get; set; }
}