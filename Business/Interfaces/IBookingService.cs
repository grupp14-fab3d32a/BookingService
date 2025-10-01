using Business.Contracts.Requests;
using Business.Contracts.Responses;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
    Task<IEnumerable<BookingResponse>?> GetAllBookingsByMemberIdAsync(Guid memberId);
    Task<BookingResponse?> GetBookingByIdAsync(Guid id);
    Task<bool> CancelBookingAsync(Guid memberId, Guid workoutId);

    Task<bool> HasAvailableSpotsAsync(string baseUrl, Guid workoutId);
    Task IncrementSpotsAsync(string baseUrl, Guid workoutId);
    Task DecrementSpotsAsync(string baseUrl, Guid workoutId);
}
