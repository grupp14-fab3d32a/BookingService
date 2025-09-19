using Business.Contracts.Requests;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<Guid> CreateBookingAsync(CreateBookingRequest request);
}
