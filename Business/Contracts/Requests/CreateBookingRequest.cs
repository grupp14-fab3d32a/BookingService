
namespace Business.Contracts.Requests;

public class CreateBookingRequest
{
    public Guid MemberId { get; set; }
    public Guid WorkoutId { get; set; }
}
