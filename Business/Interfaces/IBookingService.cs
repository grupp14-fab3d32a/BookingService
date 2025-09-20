using Business.Contracts.Requests;
using Business.Contracts.Responses;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
    Task<BookingResponse?> GetBookingByIdAsync(Guid id);
}
