using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class BookingContext(DbContextOptions<BookingContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookingEntity>().HasData(
            new BookingEntity
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                MemberId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                WorkoutId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CreatedAt = new DateTime(2025, 09, 01, 12, 00, 00, DateTimeKind.Utc),
                IsCancelled = false
            },
            new BookingEntity
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                MemberId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                WorkoutId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                CreatedAt = new DateTime(2025, 09, 01, 14, 00, 00, DateTimeKind.Utc),
                IsCancelled = false
            },
            new BookingEntity
            {
                Id = Guid.Parse("44444444-4444-4444-8888-444444444444"),
                MemberId = Guid.Parse("55555555-5555-8888-5555-555555555555"),
                WorkoutId = Guid.Parse("66666666-6666-8888-6666-666666666666"),
                CreatedAt = new DateTime(2025, 09, 01, 20, 00, 00, DateTimeKind.Utc),
                IsCancelled = true
            }
        );
    }
}