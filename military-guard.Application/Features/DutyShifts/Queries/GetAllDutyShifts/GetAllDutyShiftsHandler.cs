using MediatR;
using military_guard.Application.Features.DutyShifts.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Queries.GetAllDutyShifts
{
    public class GetAllDutyShiftsHandler : IRequestHandler<GetAllDutyShiftsQuery, List<DutyShiftResponse>>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;

        public GetAllDutyShiftsHandler(IDutyShiftRepository dutyShiftRepository)
        {
            _dutyShiftRepository = dutyShiftRepository;
        }

        public async Task<List<DutyShiftResponse>> Handle(GetAllDutyShiftsQuery request, CancellationToken cancellationToken)
        {
            var dutyShifts = await _dutyShiftRepository.GetAllShiftsAsync();

            var dtos = dutyShifts.Select(shift => new DutyShiftResponse(
                Id: shift.Id,
                StartTime: shift.StartTime,
                EndTime: shift.EndTime,
                ShiftOrder: shift.ShiftOrder
                )).ToList();

            return dtos;
        }
    }
}
