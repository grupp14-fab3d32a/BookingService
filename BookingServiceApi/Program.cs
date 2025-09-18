using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
