using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GetRegistrationById;

public sealed class GetRegistrationByIdQueryHandler(
    IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationByIdQuery, Result<GetRegistrationByIdResponse>>
{
    public async Task<Result<GetRegistrationByIdResponse>> HandleAsync(
        GetRegistrationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var registration = await dbContext.Registrations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == query.Id,
                cancellationToken);

        if (registration is null)
        {
            return Result<GetRegistrationByIdResponse>.Failure(
                Error.NotFound(
                    "Registration.NotFound",
                    "Registration was not found."));
        }

        var paymentStatus =
            registration.AmountPaid == 0 ? "Unpaid" :
            registration.AmountPaid < registration.RegistrationFee ? "Partial" :
            registration.AmountPaid == registration.RegistrationFee ? "Paid" :
            "Overpaid";

        var response = new GetRegistrationByIdResponse(
            registration.Id,
            registration.RegistrationNumber,
            registration.RegistrationDate,
            registration.ParticipantName,
            registration.ContactNumber,
            registration.Email,
            registration.Address,
            registration.CarMakeModel,
            registration.PlateNumber,
            registration.RegistrationType.ToString(),
            registration.Status.ToString(),
            registration.RegistrationFee,
            registration.AmountPaid,
            paymentStatus
        );

        return Result<GetRegistrationByIdResponse>.Success(response);
    }
}