using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IDutyShiftRepository : IGenericRepository<DutyShift>
    {
        Task<IReadOnlyList<DutyShift>> GetAllShiftsAsync();

        // Chống trùng lặp ca (ví dụ: cấm tạo 2 ca có cùng ShiftOrder)
        Task<bool> IsShiftOrderUniqueAsync(int shiftOrder, Guid? excludeId = null);

        // Phòng thủ nút Xóa
        Task<bool> HasAssignmentsAsync(Guid dutyShiftId);
    }
}
