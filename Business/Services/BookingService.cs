using System.Buffers.Text;
using Azure.Core;
using Business.Contracts.Requests;
using Business.Contracts.Responses;
using Business.Factories;
using Business.Interfaces;
using Business.Mappings;
using Data.Context;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Business.Services;

public class BookingService(IBookingRepository repository, BookingContext context, HttpClient httpClient, IConfiguration configuration) : IBookingService
{
    private readonly IBookingRepository _repository = repository;
    private readonly BookingContext _context = context;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
    {
        var baseUrl = _configuration["ScheduleServiceBaseUrl"];

        var booking = await _repository.GetAsync(x => x.MemberId == request.MemberId && x.WorkoutId == request.WorkoutId);

        //om bokning redan finns
        if (booking != null)
        {
            if (booking.IsCancelled)
            {
                // kontrollera om plats finns
                if (!await HasAvailableSpotsAsync(baseUrl, request.WorkoutId))
                    throw new InvalidOperationException("No available spots.");

                booking.IsCancelled = false;
                await _repository.UpdateAsync(booking);

                // increment spots
                await IncrementSpotsAsync(baseUrl, request.WorkoutId);

                return BookingMapper.ToResponse(booking);
            }

            throw new InvalidOperationException("Member is already booked for this workout.");
        }

        //ny bokning

        // kontrollera om plats finns
        if (!await HasAvailableSpotsAsync(baseUrl, request.WorkoutId))
            throw new InvalidOperationException("No available spots.");

        booking = BookingFactory.Create(request);
        await _repository.AddAsync(booking);
        await _context.SaveChangesAsync();

        // increment spots
        await IncrementSpotsAsync(baseUrl, request.WorkoutId);

        return BookingMapper.ToResponse(booking);
    }

    public async Task<bool> HasAvailableSpotsAsync(string baseUrl, Guid workoutId)
    {
        var response = await _httpClient.GetAsync($"{baseUrl}/api/workouts/has-available-spots/{workoutId}");
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Could not check available spots.");

        return bool.Parse(await response.Content.ReadAsStringAsync());
    }

    public async Task IncrementSpotsAsync(string baseUrl, Guid workoutId)
    {
        var response = await _httpClient.PostAsync($"{baseUrl}/api/workouts/increment/{workoutId}", null);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Could not increment spots.");
    }

    public async Task DecrementSpotsAsync(string baseUrl, Guid workoutId)
    {
        var response = await _httpClient.PostAsync($"{baseUrl}/api/workouts/decrement/{workoutId}", null);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Could not decrement spots.");
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

        var baseUrl = _configuration["ScheduleServiceBaseUrl"];

        var booking = await _repository.GetAsync(x => x.MemberId == memberId &&  x.WorkoutId == workoutId);

        if (booking == null || booking.IsCancelled)
            return false;
        
        _repository.MarkAsCancelled(booking);
        await _context.SaveChangesAsync();

        await DecrementSpotsAsync(baseUrl, workoutId);

        return true;
    }

}
