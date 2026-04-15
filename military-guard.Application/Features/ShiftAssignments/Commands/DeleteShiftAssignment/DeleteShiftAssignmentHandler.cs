using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.DeleteShiftAssignment
{
    public class DeleteShiftAssignmentHandler : IRequestHandler<DeleteShiftAssignmentCommand, Unit>
    {
        private readonly IShiftAssignmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShiftAssignmentHandler(IShiftAssignmentRepository repo, IUnitOfWork uow)
        {
            _repository = repo;
            _unitOfWork = uow;
        }

        public async Task<Unit> Handle(DeleteShiftAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _repository.GetByIdAsync(request.Id);
            if (assignment == null) throw new KeyNotFoundException("Không tìm thấy dữ liệu phân công.");

            await _repository.DeleteAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
