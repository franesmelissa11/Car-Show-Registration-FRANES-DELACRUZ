
using Application.Abstractions.Messaging;
using Application.Features.Registrations.CreateRegistration;
using Domain.Common;
using FluentValidation;
using Application.Features.GetRegistrations;
using Application.Features.GetRegistrationById;
using Application.Features.GetRegistrationSummary;
using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Registrations;
public static class RegistrationEndpoints
{
    public static IEndpointRouteBuilder MapRegistrationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/registrations")
            .WithTags("Registrations");
        group.MapGet("/", async (
            string? search,
            string? status,
            string? registrationType,
            int? page,
            int? pageSize,
            IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRegistrationsQuery(
                search,
                status,
                registrationType,
                page ?? 1,
                pageSize ?? 10);

            var result = await handler.HandleAsync(
                query, cancellationToken);

            return Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IQueryHandler<GetRegistrationByIdQuery, Result<GetRegistrationByIdResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRegistrationByIdQuery(id);

            var result = await handler.HandleAsync(
                query, cancellationToken);

            if (!result.IsSuccess)
            {
                return Results.NotFound(new
                {
                    result.Error?.Code,
                    result.Error?.Message
                });
            }

            return Results.Ok(result.Value);
        });
        group.MapGet("/summary", async (
            IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetRegistrationSummaryQuery();

            var result = await handler.HandleAsync(
                query, cancellationToken);

            return Results.Ok(result.Value);
        });


        group.MapPost("/", async (
            CreateRegistrationCommand command,
            ICommandHandler<CreateRegistrationCommand, Result<Guid>> handler,
            IValidator<CreateRegistrationCommand> validator,
            IAppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(
                command, cancellationToken);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            var result = await handler.HandleAsync(
                command, cancellationToken);

            if (!result.IsSuccess)
            {
                if (result.Error?.Code ==
                    "Registration.DuplicatePlateNumber")
                {
                    return Results.Conflict(new
                    {
                        result.Error.Code,
                        result.Error.Message
                    });
                }

                return Results.BadRequest(new
                {
                    result.Error?.Code,
                    result.Error?.Message
                });
            }

            var createdRegistration = await dbContext.Registrations
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == result.Value,
                    cancellationToken);

            return Results.Created(
                $"/api/registrations/{createdRegistration.Id}",
                new
                {
                    id = createdRegistration.Id,
                    registrationNumber = createdRegistration.RegistrationNumber,
                    registrationDate = createdRegistration.RegistrationDate,
                    registrationFee = createdRegistration.RegistrationFee
                });
        });

        return app;
    }
}
