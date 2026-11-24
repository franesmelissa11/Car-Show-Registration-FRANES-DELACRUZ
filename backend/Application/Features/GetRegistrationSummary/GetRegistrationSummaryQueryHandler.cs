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
        var registrations = dbContext.Registrations.AsNoTracking();

        var totalRegistrations = await registrations.CountAsync(
            cancellationToken);

        var pendingRegistrations = await registrations.CountAsync(
            x => x.Status == RegistrationStatus.Pending,
            cancellationToken);

        var confirmedRegistrations = await registrations.CountAsync(
            x => x.Status == RegistrationStatus.Confirmed,
            cancellationToken);

        var checkedInRegistrations = await registrations.CountAsync(
            x => x.Status == RegistrationStatus.CheckedIn,
            cancellationToken);

        var cancelledRegistrations = await registrations.CountAsync(
            x => x.Status == RegistrationStatus.Cancelled,
            cancellationToken);

        var exhibitorRegistrations = await registrations.CountAsync(
            x => x.RegistrationType == RegistrationType.Exhibitor,
            cancellationToken);

        var competitorRegistrations = await registrations.CountAsync(
            x => x.RegistrationType == RegistrationType.Competitor,
            cancellationToken);

        var vipRegistrations = await registrations.CountAsync(
            x => x.RegistrationType == RegistrationType.VIP,
            cancellationToken);

        var totalRegistrationFees = await registrations.SumAsync(
            x => (decimal?)x.RegistrationFee,
            cancellationToken) ?? 0;

        var totalAmountPaid = await registrations.SumAsync(
            x => (decimal?)x.AmountPaid,
            cancellationToken) ?? 0;

        var totalBalance = await registrations.SumAsync(
            x => (decimal?)(x.RegistrationFee - x.AmountPaid),
            cancellationToken) ?? 0;

        var response = new GetRegistrationSummaryResponse(
            TotalRegistrations: totalRegistrations,
            PendingRegistrations: pendingRegistrations,
            ConfirmedRegistrations: confirmedRegistrations,
            CheckedInRegistrations: checkedInRegistrations,
            CancelledRegistrations: cancelledRegistrations,
            ExhibitorRegistrations: exhibitorRegistrations,
            CompetitorRegistrations: competitorRegistrations,
            VIPRegistrations: vipRegistrations,
            TotalRegistrationFees: totalRegistrationFees,
            TotalAmountPaid: totalAmountPaid,
            TotalBalance: totalBalance
        );

        return Result<GetRegistrationSummaryResponse>.Success(response);
    }
}