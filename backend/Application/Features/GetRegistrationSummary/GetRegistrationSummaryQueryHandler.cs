
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GetRegistrationSummary;

public sealed class GetRegistrationSummaryQueryHandler(
    IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>>
{
    public async Task<Result<GetRegistrationSummaryResponse>> HandleAsync(
        GetRegistrationSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        var registrations = await dbContext.Registrations
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = new GetRegistrationSummaryResponse(
            TotalRegistrations: registrations.Count,

            PendingRegistrations: registrations.Count(
                x => x.Status == RegistrationStatus.Pending),

            ConfirmedRegistrations: registrations.Count(
                x => x.Status == RegistrationStatus.Confirmed),

            CheckedInRegistrations: registrations.Count(
                x => x.Status == RegistrationStatus.CheckedIn),

            CancelledRegistrations: registrations.Count(
                x => x.Status == RegistrationStatus.Cancelled),

            ExhibitorRegistrations: registrations.Count(
                x => x.RegistrationType == RegistrationType.Exhibitor),

            CompetitorRegistrations: registrations.Count(
                x => x.RegistrationType == RegistrationType.Competitor),

            VIPRegistrations: registrations.Count(
                x => x.RegistrationType == RegistrationType.VIP),

            TotalRegistrationFees: registrations.Sum(
                x => x.RegistrationFee),

            TotalAmountPaid: registrations.Sum(
                x => x.AmountPaid),

            TotalBalance: registrations.Sum(
                x => x.RegistrationFee - x.AmountPaid)
        );

        return Result<GetRegistrationSummaryResponse>.Success(response);
    }
}
