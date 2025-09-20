using Business.Contracts.Requests;
using Business.Contracts.Responses;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<Guid> CreateBookingAsync(CreateBookingRequest request);
    Task<BookingResponse?> GetBookingByIdAsync(Guid id);
}
