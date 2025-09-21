using Business.Contracts.Requests;
using Data.Entities;

namespace Business.Factories;

public static class BookingFactory
{
    public static BookingEntity Create(CreateBookingRequest request)
    {
        return new BookingEntity
        {
            Id = Guid.NewGuid(),
            MemberId = request.MemberId,
            WorkoutId = request.WorkoutId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
