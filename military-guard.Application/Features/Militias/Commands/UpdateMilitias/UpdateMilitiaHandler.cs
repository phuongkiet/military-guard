using military_guard.Application.Interfaces;
using MediatR;

namespace military_guard.Application.Features.Militias.Commands.UpdateMilitias;

public class UpdateMilitiaHandler : IRequestHandler<UpdateMilitiaCommand, Unit>
{
    private readonly IMilitiaRepository _militiaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMilitiaHandler(IMilitiaRepository repo, IUnitOfWork uow)
    {
        _militiaRepository = repo;
        _unitOfWork = uow;
    }

    public async Task<Unit> Handle(UpdateMilitiaCommand request, CancellationToken cancellationToken)
    {
        var militia = await _militiaRepository.GetByIdAsync(request.Id);
        if (militia == null)
            throw new KeyNotFoundException($"Không tìm thấy dân quân ID: {request.Id}");

        bool isUnique = await _militiaRepository.IsEmailUniqueAsync(request.Email, request.Id);
        if (!isUnique)
            throw new Exception($"Email '{request.Email}' đã được người khác sử dụng.");

        if (request.ManagerId.HasValue)
        {
            bool isValidManager = await _militiaRepository.IsValidManagerAsync(request.ManagerId.Value, request.Id);
            if (!isValidManager)
            {
                throw new Exception("Chỉ huy được chọn không hợp lệ (Không tồn tại, đã xuất ngũ, hoặc gây ra vòng lặp chỉ huy).");
            }
        }

        // 4. Cập nhật dữ liệu
        militia.FullName = request.FullName;
        militia.Email = request.Email;
        militia.Type = request.Type;
        militia.Rank = request.Rank;
        militia.JoinDate = request.JoinDate;
        militia.ManagerId = request.ManagerId;

        // 5. Lưu Database
        await _militiaRepository.UpdateAsync(militia);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}