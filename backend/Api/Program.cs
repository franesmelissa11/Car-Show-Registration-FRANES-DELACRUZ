using Api.Features.Registrations;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.GetRegistrations;
using Application.Features.GetRegistrationById;
using Application.Features.Registrations.CreateRegistration;
using Domain.Common;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Application.Features.GetRegistrationSummary;
using Domain.Entities;
using Domain.Enums;

var builder = WebApplication.CreateBuilder(args);
// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Database
var databasePath = Path.GetFullPath(
    Path.Combine(
        AppContext.BaseDirectory,
        "..",
        "..",
        "..",
        "carshow.db"));

Console.WriteLine("DATABASE PATH: " + databasePath);
Console.WriteLine("DATABASE EXISTS: " + File.Exists(databasePath));

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

// Validators
builder.Services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);

// Handlers
builder.Services.AddScoped<
    ICommandHandler<CreateRegistrationCommand, Result<Guid>>,
    CreateRegistrationCommandHandler>();
builder.Services.AddScoped<
    IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>>,
    GetRegistrationsQueryHandler>();
builder.Services.AddScoped<
    IQueryHandler<GetRegistrationByIdQuery, Result<GetRegistrationByIdResponse>>,
    GetRegistrationByIdQueryHandler>();
builder.Services.AddScoped<
    IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>>,
    GetRegistrationSummaryQueryHandler>();
// API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Car Show Registration System API";
        document.Info.Description =
            "Backend API for managing car show registrations, participant information, registration types, payment details, and registration summaries.";

        document.Info.Version = "v1";

        return Task.CompletedTask;
    });
});

var app = builder.Build();
// Enable CORS
app.UseCors("ReactFrontend");


// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    for (int number = 1; number <= 25; number++)
    {
        var registrationNumber = $"CS27-{number:0000}";

        // Prevent duplicate sample registrations
        if (dbContext.Registrations.Any(
            x => x.RegistrationNumber == registrationNumber))
        {
            continue;
        }

        var registrationType = (number % 3) switch
        {
            0 => RegistrationType.Exhibitor,
            1 => RegistrationType.Competitor,
            _ => RegistrationType.VIP
        };

        decimal fee = registrationType switch
        {
            RegistrationType.Exhibitor => 1500m,
            RegistrationType.Competitor => 2500m,
            RegistrationType.VIP => 5000m,
            _ => 0m
        };

        decimal amountPaid = (number % 3) switch
        {
            0 => fee,
            1 => fee / 2,
            _ => 0m
        };

        var registration = new Registration
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = registrationNumber,
            RegistrationDate = DateTime.UtcNow.AddDays(-number),
            ParticipantName = $"Sample Participant {number:00}",
            ContactNumber = $"0917{number:0000000}",
            Email = $"participant{number}@example.com",
            Address = "Sorsogon City",
            CarMakeModel = $"Toyota Vios {2018 + number % 8}",
            PlateNumber = $"SMP{number:0000}",
            RegistrationType = registrationType,
            Status = (number % 4) switch
            {
                0 => RegistrationStatus.Pending,
                1 => RegistrationStatus.Confirmed,
                2 => RegistrationStatus.CheckedIn,
                _ => RegistrationStatus.Cancelled
            },
            RegistrationFee = fee,
            AmountPaid = amountPaid
        };

        dbContext.Registrations.Add(registration);
    }

    dbContext.SaveChanges();
}


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Root endpoint
app.MapGet("/", () => "Welcome to the Vertical Slice API! Go to /scalar/v1 for API documentation.");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    message = "Car Show Registration System API is running."
}))
.WithTags("Health");

// Map endpoints
app.MapRegistrationEndpoints();

app.Run();
