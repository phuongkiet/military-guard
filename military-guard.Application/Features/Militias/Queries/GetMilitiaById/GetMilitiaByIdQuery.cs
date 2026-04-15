using MediatR;
using military_guard.Application.Features.Militias.DTOs;

namespace military_guard.Application.Features.Militias.Queries.GetMilitiaById;

public record GetMilitiaByIdQuery(Guid Id) : IRequest<MilitiaResponse>;

