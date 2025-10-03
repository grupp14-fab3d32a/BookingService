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
using Microsoft.AspNetCore.Http;

namespace Business.Services;

public class BookingService(IBookingRepository repository, BookingContext context, HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IBookingService
{
    private readonly IBookingRepository _repository = repository;
    private readonly BookingContext _context = context;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
        var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/api/workouts/has-available-spots/{workoutId}");
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", token);
        }

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Could not check available spots. Status: {response.StatusCode}");

        return bool.Parse(await response.Content.ReadAsStringAsync());
    }

    public async Task IncrementSpotsAsync(string baseUrl, Guid workoutId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/workouts/increment/{workoutId}");
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", token);
        }
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Could not increment spots. Status: {response.StatusCode}");
    }

    public async Task DecrementSpotsAsync(string baseUrl, Guid workoutId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/workouts/decrement/{workoutId}");
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", token);
        }
        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Could not decrement spots. Status: {response.StatusCode}");
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
