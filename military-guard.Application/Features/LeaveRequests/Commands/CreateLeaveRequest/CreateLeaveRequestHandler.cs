using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestHandler : IRequestHandler<CreateLeaveRequestCommand, Guid>
    {
        private readonly ILeaveRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveRequestHandler(ILeaveRequestRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            bool isOverlapping = await _repository.HasOverlappingLeaveAsync(request.MilitiaId, request.StartDate, request.EndDate);

            if (isOverlapping)
            {
                throw new Exception("Dân quân này đã có đơn xin nghỉ phép khác (Đang chờ hoặc Đã duyệt) trùng với khoảng thời gian này.");
            }

            var leaveRequest = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                MilitiaId = request.MilitiaId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Reason = request.Reason,
                Status = LeaveStatus.Pending 
            };

            await _repository.AddAsync(leaveRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return leaveRequest.Id;
        }
    }
}
