using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.Militias.DTOs;
using military_guard.Domain.Enums;

namespace military_guard.Application.Features.Militias.Queries.GetMilitias;

public record GetAllMilitiasQuery : IRequest<PaginatedList<MilitiaResponse>>
{
    // Điều kiện tìm kiếm bằng chữ
    public string? SearchTerm { get; init; }

    // Điều kiện lọc theo Dropdown (Combobox) ở UI
    public MilitiaType? Type { get; init; }
    public MilitiaRank? Rank { get; init; }

    // Tham số phân trang
    public int PageIndex { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}