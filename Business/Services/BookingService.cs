using Business.Contracts.Requests;
using Business.Interfaces;
using Data.Context;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class BookingService(IBookingRepository repository, BookingContext context) : IBookingService
{
    private readonly IBookingRepository _repository = repository;
    private readonly BookingContext _context = context;

    public Task<Guid> CreateBookingAsync(CreateBookingRequest request)
    {
        throw new NotImplementedException();
    }
}

