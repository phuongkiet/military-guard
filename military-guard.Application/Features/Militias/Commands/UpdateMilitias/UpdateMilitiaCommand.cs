using MediatR;
using military_guard.Domain.Enums;

namespace military_guard.Application.Features.Militias.Commands.UpdateMilitias;

public class UpdateMilitiaCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Các trường đặc thù của Quân sự
    public MilitiaType Type { get; set; }
    public MilitiaRank Rank { get; set; }
    public DateTime JoinDate { get; set; }

    public Guid? ManagerId { get; set; }
}
