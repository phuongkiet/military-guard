using MediatR;
using military_guard.Application.Features.Attendances.DTOs;
using military_guard.Application.Interfaces;

namespace military_guard.Application.Features.Attendances.Queries.GetLiveAttendances
{
    public class GetLiveAttendancesHandler : IRequestHandler<GetLiveAttendancesQuery, List<LiveAttendanceDto>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IShiftAssignmentRepository _shiftAssignmentRepository;

        public GetLiveAttendancesHandler(
            IAttendanceRepository attendanceRepository,
            IShiftAssignmentRepository shiftAssignmentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _shiftAssignmentRepository = shiftAssignmentRepository;
        }

        public async Task<List<LiveAttendanceDto>> Handle(GetLiveAttendancesQuery request, CancellationToken cancellationToken)
        {
            // 1. Xác định ngày cần lấy (Mặc định là hôm nay)
            var targetDate = request.Date ?? DateOnly.FromDateTime(DateTime.Now);

            // 2. Lấy danh sách phân công qua Interface
            var assignments = await _shiftAssignmentRepository.GetAssignmentsByShiftAndDateAsync(request.ShiftId, targetDate);

            // 3. Lấy lịch sử điểm danh qua Interface
            var attendances = await _attendanceRepository.GetAttendancesByShiftAndDateAsync(request.ShiftId, targetDate);

            var result = assignments.Select(sa =>
            {
                // Tìm xem người này đã điểm danh chưa
                var attendanceRecord = attendances.FirstOrDefault(a => a.MilitiaId == sa.MilitiaId);

                return new LiveAttendanceDto
                {
                    MilitiaId = sa.MilitiaId,
                    FullName = sa.Militia!.FullName,
                    GuardPostName = sa.GuardPost?.Name ?? "Trụ sở",
                    DutyShiftName = "Ca " + (sa.DutyShift?.ShiftOrder ?? null),
                    IsLeader = sa.IsLeader,
                    IsStandby = sa.IsStandby,
                    Note = attendanceRecord?.Note ?? "Không có",

                    // Nối dữ liệu điểm danh (nếu có)
                    AttendanceId = attendanceRecord?.Id,
                    CheckInTime = attendanceRecord?.CheckInTime,
                    Status = attendanceRecord?.Status
                };
            })
            .OrderBy(x => x.GuardPostName) // Gom nhóm theo chốt cho dễ nhìn
            .ThenByDescending(x => x.IsLeader)
            .ToList();

            return result;
        }
    }
}
