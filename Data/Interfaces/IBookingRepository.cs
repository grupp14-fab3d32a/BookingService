using System.Linq.Expressions;
using Data.Entities;

namespace Data.Interfaces;

public interface IBookingRepository
{
    Task AddAsync (BookingEntity booking);
    Task<IEnumerable<BookingEntity>> GetAllAsync(Expression<Func<BookingEntity, bool>> predicate = null!, Func<IQueryable<BookingEntity>, IQueryable<BookingEntity>>? includeExpression = null);
    Task<BookingEntity?> GetByIdAsync (Guid id);
    Task<BookingEntity?> GetAsync(Expression<Func<BookingEntity, bool>> expression);
    Task<BookingEntity?> UpdateAsync(BookingEntity entity);
    Task<bool> ExistsAsync(Guid workoutId, Guid memberId);
    void MarkAsCancelled(BookingEntity booking);
}
