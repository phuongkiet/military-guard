using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.DTOs
{
    public class SubstituteDto
    {
        public Guid MilitiaId { get; set; }
        public string FullName { get; set; }
        public bool IsStandby { get; set; } // Để UI biết ông này đang ngồi chơi hay đang trực dự bị
    }
}
