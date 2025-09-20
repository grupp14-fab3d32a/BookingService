using Business.Contracts.Requests;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var bookingId = await _bookingService.CreateBookingAsync(request);

        return CreatedAtAction(nameof(GetBookingById), new { id = bookingId }, new { id = bookingId });

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        var response = await _bookingService.GetBookingByIdAsync(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }
}
