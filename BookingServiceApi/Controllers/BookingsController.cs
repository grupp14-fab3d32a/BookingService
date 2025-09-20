using Azure;
using Business.Contracts.Requests;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingServiceApi.Controllers;

[Route("api/[controller]")]

//[Authorize(Roles = "Member")] uncomment this line to enable authorization !!!!!!!!!!!!!!!!!!!!
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _bookingService.CreateBookingAsync(request);
            return CreatedAtAction(nameof(GetBookingById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); // HTTP 409 Conflict
        }
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
