
using Application.Abstractions.Messaging;
using Domain.Common;

namespace Application.Features.GetRegistrations;

public sealed record GetRegistrationsQuery(
    string? Search = null,
    string? Status = null,
    string? RegistrationType = null,
    int Page = 1,
    int PageSize = 10
) : IQuery<Result<GetRegistrationsResponse>>;

public sealed record GetRegistrationsResponse(
    IReadOnlyList<RegistrationListItem> Registrations,
    int Page,
    int PageSize,
    int TotalCount
);

public sealed record RegistrationListItem(
    Guid Id,
    string RegistrationNumber,
    DateTime RegistrationDate,
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    string RegistrationType,
    string Status,
    decimal RegistrationFee,
    decimal AmountPaid,
    string PaymentStatus
);
