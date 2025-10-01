using System.Linq.Expressions;
using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BookingRepository(BookingContext context) : IBookingRepository
{
    private readonly BookingContext _context = context;

    #region Create
    public async Task AddAsync(BookingEntity booking)
    {
        await _context.Bookings.AddAsync(booking);
    }
    #endregion

    #region Read
    public async Task<IEnumerable<BookingEntity>> GetAllAsync(Expression<Func<BookingEntity, bool>> predicate = null!, Func<IQueryable<BookingEntity>, IQueryable<BookingEntity>>? includeExpression = null)
    {
        IQueryable<BookingEntity> query = _context.Bookings;
        if (includeExpression != null)
            query = includeExpression(query);

        if(predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<BookingEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<BookingEntity?> GetAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        return await _context.Bookings.FirstOrDefaultAsync(expression);
    }
    #endregion

    #region Update
    public async Task<BookingEntity?> UpdateAsync(BookingEntity entity)
    {
        _context.Bookings.Update(entity);
        var updated = await _context.SaveChangesAsync();
        return updated > 0 ? entity : null;
    }
    #endregion

    public async Task<bool> ExistsAsync(Guid workoutId, Guid memberId)
    {
        return await _context.Bookings.AnyAsync(b =>
            b.WorkoutId == workoutId &&
            b.MemberId == memberId);
    }

    public void MarkAsCancelled(BookingEntity booking)
    {
        booking.IsCancelled = true;
    }
}
