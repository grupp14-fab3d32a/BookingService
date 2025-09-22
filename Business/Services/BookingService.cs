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
        var alreadyBooked = await _repository.ExistsAsync(request.WorkoutId, request.MemberId);
        if (alreadyBooked)
            throw new InvalidOperationException("Member is already booked for this workout.");

        var booking = BookingFactory.Create(request);

        await _repository.AddAsync(booking);
        await _context.SaveChangesAsync();

        return BookingMapper.ToResponse(booking);
    }

    public async Task<BookingResponse?> GetBookingByIdAsync(Guid id)
    {
        var booking = await _repository.GetByIdAsync(id);

        if (booking == null)
            return null;    

        return BookingMapper.ToResponse(booking);  
    }

    public async Task<bool> CancelBookingAsync(Guid id)
    {
        //Add business logic like time restrictions for cancelling here if needed

        var booking = await _repository.GetByIdAsync(id);

        if (booking == null || booking.IsCancelled)
            return false;
        
        _repository.MarkAsCancelled(booking);
        await _context.SaveChangesAsync();

        return true;
    }

}
