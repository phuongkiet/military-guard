using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestHandler : IRequestHandler<DeleteLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository, IUnitOfWork unitOfWork)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest == null)
                throw new KeyNotFoundException("Không tìm thấy đơn.");

            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                throw new Exception("Không thể hủy đơn đã qua xử lý. Vui lòng liên hệ cấp trên.");
            }

            await _leaveRequestRepository.DeleteAsync(leaveRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
