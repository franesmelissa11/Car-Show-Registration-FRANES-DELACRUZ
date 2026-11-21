
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Registrations.CreateRegistration;

public sealed class CreateRegistrationCommandHandler(
    IAppDbContext dbContext)
    : ICommandHandler<CreateRegistrationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(
        CreateRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        // Check for duplicate plate number
        var plateNumber = command.PlateNumber.Trim().ToUpperInvariant();

        var plateExists = await dbContext.Registrations
            .AnyAsync(x => x.PlateNumber == plateNumber, cancellationToken);

        if (plateExists)
        {
            return Result<Guid>.Failure(
                Error.Validation(
                    "Registration.DuplicatePlateNumber",
                    "This plate number is already registered."));
        }

        // Set registration fee based on registration type
        decimal fee = command.RegistrationType switch
        {
            RegistrationType.Exhibitor => 1500m,
            RegistrationType.Competitor => 2500m,
            RegistrationType.VIP => 5000m,
            _ => 0m
        };

        if (fee == 0m)
        {
            return Result<Guid>.Failure(
                Error.Validation(
                    "Registration.InvalidType",
                    "Invalid registration type."));
        }

        // Validate amount paid
        if (command.AmountPaid < 0 || command.AmountPaid > fee)
        {
            return Result<Guid>.Failure(
                Error.Validation(
                    "Registration.InvalidAmount",
                    "Amount paid must be between zero and the registration fee."));
        }

        // Generate registration number
        var lastRegistrationNumber = await dbContext.Registrations
            .OrderByDescending(x => x.RegistrationNumber)
            .Select(x => x.RegistrationNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;

        if (!string.IsNullOrEmpty(lastRegistrationNumber)
            && lastRegistrationNumber.StartsWith("CS27-")
            && int.TryParse(lastRegistrationNumber[5..], out var lastNumber))
        {
            nextNumber = lastNumber + 1;
        }

        var registrationNumber = $"CS27-{nextNumber:0000}";

        // Create registration
        var registration = new Registration
        {
            RegistrationNumber = registrationNumber,
            RegistrationDate = DateTime.UtcNow,
            ParticipantName = command.ParticipantName.Trim(),
            ContactNumber = command.ContactNumber.Trim(),
            Email = command.Email.Trim(),
            Address = command.Address.Trim(),
            CarMakeModel = command.CarMakeModel.Trim(),
            PlateNumber = plateNumber,
            RegistrationType = command.RegistrationType,
            Status = RegistrationStatus.Pending,
            RegistrationFee = fee,
            AmountPaid = command.AmountPaid
        };

        dbContext.Registrations.Add(registration);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(registration.Id);
    }
}
