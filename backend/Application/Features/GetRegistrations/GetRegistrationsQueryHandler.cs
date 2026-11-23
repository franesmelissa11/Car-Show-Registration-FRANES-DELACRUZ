
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GetRegistrations;

public sealed class GetRegistrationsQueryHandler(
    IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>>
{
    public async Task<Result<GetRegistrationsResponse>> HandleAsync(
        GetRegistrationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var registrations = dbContext.Registrations.AsNoTracking();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            registrations = registrations.Where(x =>
                x.ParticipantName.Contains(search) ||
                x.RegistrationNumber.Contains(search) ||
                x.PlateNumber.Contains(search) ||
                x.CarMakeModel.Contains(search));
        }

        // Filter by status
        if (!string.IsNullOrWhiteSpace(query.Status) &&
            Enum.TryParse<Domain.Enums.RegistrationStatus>(
                query.Status, true, out var status))
        {
            registrations = registrations.Where(x => x.Status == status);
        }

        // Filter by registration type
        if (!string.IsNullOrWhiteSpace(query.RegistrationType) &&
            Enum.TryParse<Domain.Enums.RegistrationType>(
                query.RegistrationType, true, out var type))
        {
            registrations = registrations.Where(
                x => x.RegistrationType == type);
        }

        var totalCount = await registrations.CountAsync(cancellationToken);

        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await registrations
            .OrderByDescending(x => x.RegistrationDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var response = items.Select(x => new RegistrationListItem(
            x.Id,
            x.RegistrationNumber,
            x.RegistrationDate,
            x.ParticipantName,
            x.ContactNumber,
            x.Email,
            x.Address,
            x.CarMakeModel,
            x.PlateNumber,
            x.RegistrationType.ToString(),
            x.Status.ToString(),
            x.RegistrationFee,
            x.AmountPaid,
            x.AmountPaid == 0 ? "Unpaid" :
            x.AmountPaid < x.RegistrationFee ? "Partial" : "Paid"
        )).ToList();

        return Result<GetRegistrationsResponse>.Success(
         new GetRegistrationsResponse(
         response,
         page,
         pageSize,
         totalCount,
         totalPages));
    }
}
