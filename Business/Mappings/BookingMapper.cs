using Business.Contracts.Responses;
using Data.Entities;

namespace Business.Mappings;

public static class BookingMapper
{
    public static BookingResponse ToResponse(BookingEntity entity)
    {
        return new BookingResponse
        {
            Id = entity.Id,
            MemberId = entity.MemberId,
            WorkoutId = entity.WorkoutId,
            CreatedAt = entity.CreatedAt,
            IsCancelled = entity.IsCancelled
        };
    }
}
