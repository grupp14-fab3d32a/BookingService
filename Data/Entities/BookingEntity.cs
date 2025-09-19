using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class BookingEntity
{
    [Key]
    Guid Id { get; set; }         //Primary key

    [Required]
    Guid MemberId { get; set; }   //External reference to UserService

    [Required]
    Guid WorkoutId { get; set; }  //External reference to ScheduleService

    DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
