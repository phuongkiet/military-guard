using military_guard.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Employees.Commands.DeleteMilitias
{
    public class DeleteMilitiaHandler : IRequestHandler<DeleteMilitiaCommand, Unit>
    {
        private readonly IMilitiaRepository _militiaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMilitiaHandler(IMilitiaRepository repo, IUnitOfWork uow)
        {
            _militiaRepository = repo;
            _unitOfWork = uow;
        }

        public async Task<Unit> Handle(DeleteMilitiaCommand request, CancellationToken cancellationToken)
        {
            var employee = await _militiaRepository.GetByIdAsync(request.Id);
            if (employee == null)
                throw new KeyNotFoundException("Nhân viên không tồn tại hoặc đã bị xóa.");

            // ĐIỂM ĂN TIỀN: Không gọi _militiaRepository.DeleteAsync()
            // Thay vào đó, ta đổi cờ trạng thái
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.UtcNow;

            // Cập nhật thay vì Xóa
            await _militiaRepository.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
