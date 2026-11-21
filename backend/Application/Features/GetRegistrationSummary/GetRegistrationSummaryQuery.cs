
using Application.Abstractions.Messaging;
using Domain.Common;

namespace Application.Features.GetRegistrationSummary;

public sealed record GetRegistrationSummaryQuery()
    : IQuery<Result<GetRegistrationSummaryResponse>>;

public sealed record GetRegistrationSummaryResponse(
    int TotalRegistrations,
    int PendingRegistrations,
    int ConfirmedRegistrations,
    int CheckedInRegistrations,
    int CancelledRegistrations,
    int ExhibitorRegistrations,
    int CompetitorRegistrations,
    int VIPRegistrations,
    decimal TotalRegistrationFees,
    decimal TotalAmountPaid,
    decimal TotalBalance
);
