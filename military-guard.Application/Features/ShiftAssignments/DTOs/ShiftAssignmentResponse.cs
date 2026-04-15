using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.DTOs
{
    public record ShiftAssignmentResponse(
    Guid Id,
    Guid MilitiaId,
    string MilitiaName,
    string MilitiaRank,
    Guid GuardPostId,
    string GuardPostName,
    Guid DutyShiftId,
    string DutyShiftInfo, // Ví dụ: "Ca 1 (06:00 - 12:00)"
    DateOnly Date,
    bool IsLeader
);
}
