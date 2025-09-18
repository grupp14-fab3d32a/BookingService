namespace Data.Entities
{
    public class BookingEntity
    {
        Guid Id { get; set; }
        Guid MemberId { get; set; }
        Guid WorkoutId { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
