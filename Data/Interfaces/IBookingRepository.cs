using Data.Entities;

namespace Data.Interfaces;

public interface IBookingRepository
{
    Task AddAsync (BookingEntity booking);
    Task<BookingEntity?> GetByIdAsync (Guid id);

}
