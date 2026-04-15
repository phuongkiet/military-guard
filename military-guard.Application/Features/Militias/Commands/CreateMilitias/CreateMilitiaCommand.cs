using MediatR;
using military_guard.Domain.Enums;

namespace military_guard.Application.Features.Militias.Commands.CreateMilitias;

public record CreateMilitiaCommand(
    string FullName,
    string Email,
    MilitiaType Type,    
    MilitiaRank Rank,    
    DateTime JoinDate,   
    Guid? ManagerId) : IRequest<Guid>;