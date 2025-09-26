using Business.Contracts.Requests;
using Business.Contracts.Responses;
using Business.Factories;
using Business.Interfaces;
using Business.Mappings;
using Data.Context;
using Data.Interfaces;

namespace Business.Services;

public class BookingService(IBookingRepository repository, BookingContext context) : IBookingService
{
    private readonly IBookingRepository _repository = repository;
    private readonly BookingContext _context = context;

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
    {
        var booking = await _repository.GetAsync(x => x.MemberId == request.MemberId && x.WorkoutId == request.WorkoutId);

        if (booking != null)
        {
            if (booking.IsCancelled)
            {
                booking.IsCancelled = false;
                await _repository.UpdateAsync(booking);
                return BookingMapper.ToResponse(booking);
            }

            throw new InvalidOperationException("Member is already booked for this workout.");
        }

        booking = BookingFactory.Create(request);

        await _repository.AddAsync(booking);
        await _context.SaveChangesAsync();

        return BookingMapper.ToResponse(booking);
    }

    public async Task<IEnumerable<BookingResponse>?> GetAllBookingsByMemberIdAsync(Guid memberId)
    {
        var bookings = await _repository.GetAllAsync(x => x.MemberId == memberId);

        if (bookings == null)
            return null;

        return bookings.Select(BookingMapper.ToResponse).ToList();
    }
    public async Task<BookingResponse?> GetBookingByIdAsync(Guid id)
    {
        var booking = await _repository.GetByIdAsync(id);

        if (booking == null)
            return null;    

        return BookingMapper.ToResponse(booking);  
    }

    public async Task<bool> CancelBookingAsync(Guid memberId, Guid workoutId)
    {
        //Add business logic like time restrictions for cancelling here if needed

        var booking = await _repository.GetAsync(x => x.MemberId == memberId &&  x.WorkoutId == workoutId);

        if (booking == null || booking.IsCancelled)
            return false;
        
        _repository.MarkAsCancelled(booking);
        await _context.SaveChangesAsync();

        return true;
    }

}
