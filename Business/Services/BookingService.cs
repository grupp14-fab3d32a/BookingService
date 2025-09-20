using Business.Contracts.Requests;
using Business.Factories;
using Business.Interfaces;
using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class BookingService(IBookingRepository repository, BookingContext context) : IBookingService
{
    private readonly IBookingRepository _repository = repository;
    private readonly BookingContext _context = context;

    public async Task<Guid> CreateBookingAsync(CreateBookingRequest request)
    {
        var booking = BookingFactory.Create(request);

        await _repository.AddAsync(booking);
        await _context.SaveChangesAsync();

        return booking.Id;
    }
}

