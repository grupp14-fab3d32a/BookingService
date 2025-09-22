using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BookingRepository(BookingContext context) : IBookingRepository
{
    private readonly BookingContext _context = context;

    public async Task AddAsync(BookingEntity booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public async Task<BookingEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid workoutId, Guid memberId)
    {
        return await _context.Bookings.AnyAsync(b =>
            b.WorkoutId == workoutId &&
            b.MemberId == memberId);
    }

    public async Task<bool> CancelBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null || booking.IsCancelled)
        {
            return false; // Booking not found or already cancelled
        }
        booking.IsCancelled = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
