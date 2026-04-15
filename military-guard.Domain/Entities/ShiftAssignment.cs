using military_guard.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Domain.Entities
{
    public class ShiftAssignment : BaseEntity
    {
        // Liên kết Dân quân
        public Guid MilitiaId { get; set; }
        public Militia? Militia { get; set; }

        // Liên kết Chốt trực
        public Guid GuardPostId { get; set; }
        public GuardPost? GuardPost { get; set; }

        // Liên kết Khung giờ
        public Guid DutyShiftId { get; set; }
        public DutyShift? DutyShift { get; set; }

        // Ngày trực cụ thể (DateOnly sinh ra để giải quyết bài toán lịch)
        public DateOnly Date { get; set; }

        // Thuật toán sẽ tick cờ này lên true nếu chọn anh này làm Leader của ca đó
        public bool IsLeader { get; set; } = false;
    }
}
