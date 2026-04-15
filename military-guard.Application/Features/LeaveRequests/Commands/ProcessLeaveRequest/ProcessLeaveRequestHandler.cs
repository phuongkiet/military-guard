using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.ProcessLeaveRequest
{
    public class ProcessLeaveRequestHandler : IRequestHandler<ProcessLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository, IUnitOfWork unitOfWork)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ProcessLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.NewStatus == LeaveStatus.Pending)
            {
                throw new ArgumentException("Trạng thái duyệt không hợp lệ. Chỉ có thể Chấp nhận (Approved) hoặc Từ chối (Rejected).");
            }

            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
            if (leaveRequest == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn xin nghỉ phép với ID: {request.LeaveRequestId}");
            }

            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                throw new InvalidOperationException($"Không thể xử lý đơn này vì nó đã được xử lý (Trạng thái hiện tại: {leaveRequest.Status}).");
            }

            leaveRequest.Status = request.NewStatus;

            await _leaveRequestRepository.UpdateAsync(leaveRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
