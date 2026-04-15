using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.Militias.DTOs;
using military_guard.Application.Interfaces;

namespace military_guard.Application.Features.Militias.Queries.GetMilitias;

public class GetMilitiasQueryHandler : IRequestHandler<GetAllMilitiasQuery, PaginatedList<MilitiaResponse>>
{
    private readonly IMilitiaRepository _militiaRepository;

    public GetMilitiasQueryHandler(IMilitiaRepository militiaRepository)
    {
        _militiaRepository = militiaRepository;
    }

    public async Task<PaginatedList<MilitiaResponse>> Handle(GetAllMilitiasQuery request, CancellationToken cancellationToken)
    {
        // 1. Gọi Repo với các tham số lọc mới
        var pagedEntities = await _militiaRepository.GetPagedMilitiasAsync(
            request.SearchTerm,
            request.Type,
            request.Rank,
            request.PageIndex,
            request.PageSize);

        // 2. Map từ Entity sang DTO
        var dtos = pagedEntities.Items.Select(m => new MilitiaResponse(
            m.Id,
            m.FullName,
            m.Email,
            m.Type.ToString(),         // Chuyển Enum thành chữ
            m.Rank.ToString(),         // Chuyển Enum thành chữ
            m.MonthsOfService,         // Lấy giá trị tính toán tự động từ Entity
            m.Manager?.FullName
        )).ToList();

        // 3. Đóng gói phân trang và trả về
        return new PaginatedList<MilitiaResponse>(
            dtos,
            pagedEntities.TotalCount,
            request.PageIndex,
            request.PageSize);
    }
}