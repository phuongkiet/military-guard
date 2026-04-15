using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Militias.DTOs
{
    public record MilitiaResponse(
    Guid Id,
    string FullName,
    string Email,
    string Type,             // Loại dân quân (Thường trực/Cơ động)
    string Rank,             // Cấp bậc
    int MonthsOfService,     // ĐIỂM ĂN TIỀN: Trả về số tháng thâm niên
    string? ManagerName      // Tên người chỉ huy
);
}
