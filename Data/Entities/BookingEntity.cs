using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class BookingEntity
{
    [Key]
    public Guid Id { get; set; }         //Primary key

    [Required]
    public Guid MemberId { get; set; }   //External reference to UserService

    [Required]
    public Guid WorkoutId { get; set; }  //External reference to ScheduleService

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
