using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class BookingContext(DbContextOptions<BookingContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
}