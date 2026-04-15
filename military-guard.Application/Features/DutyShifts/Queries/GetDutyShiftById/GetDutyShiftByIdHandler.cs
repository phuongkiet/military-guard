using MediatR;
using military_guard.Application.Features.DutyShifts.DTOs;
using military_guard.Application.Features.GuardPosts.DTOs;
using military_guard.Application.Features.GuardPosts.Queries.GetGuardPostById;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Queries.GetDutyShiftById
{
    public class GetDutyShiftByIdHandler : IRequestHandler<GetDutyShiftByIdQuery, DutyShiftResponse>
    {
        private readonly IDutyShiftRepository _dutyShiftRepository;
        public GetDutyShiftByIdHandler(IDutyShiftRepository dutyShiftRepository)
        {
            _dutyShiftRepository = dutyShiftRepository;
        }

        public async Task<DutyShiftResponse> Handle(GetDutyShiftByIdQuery request, CancellationToken cancellationToken)
        {
            var dutyShift = await _dutyShiftRepository.GetByIdAsync(request.Id);

            if (dutyShift == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy ca trực với ID: {request.Id}");
            }

            return new DutyShiftResponse(
                Id: dutyShift.Id,
                StartTime: dutyShift.StartTime,
                EndTime: dutyShift.EndTime,
                ShiftOrder: dutyShift.ShiftOrder
            );
        }
    }
}
