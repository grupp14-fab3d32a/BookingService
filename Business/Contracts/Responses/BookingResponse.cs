namespace Business.Contracts.Responses
{
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Guid WorkoutId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
