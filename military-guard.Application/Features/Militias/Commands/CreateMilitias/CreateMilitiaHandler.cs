using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using MediatR;

namespace military_guard.Application.Features.Militias.Commands.CreateMilitias;

public class CreateMilitiaCommandHandler : IRequestHandler<CreateMilitiaCommand, Guid>
{
    private readonly IMilitiaRepository _militiaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMilitiaCommandHandler(IMilitiaRepository militiaRepository, IUnitOfWork unitOfWork)
    {
        _militiaRepository = militiaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateMilitiaCommand request, CancellationToken cancellationToken)
    {
        bool isUnique = await _militiaRepository.IsEmailUniqueAsync(request.Email);
        if (!isUnique)
        {
            throw new Exception($"Email {request.Email} đã tồn tại trong hệ thống.");
        }

        var newMilitiaId = Guid.NewGuid();

        if (request.ManagerId.HasValue)
        {
            bool isValidManager = await _militiaRepository.IsValidManagerAsync(request.ManagerId.Value, newMilitiaId);
            if (!isValidManager)
            {
                throw new Exception("Chỉ huy được chọn không tồn tại hoặc đã bị xóa khỏi hệ thống.");
            }
        }

        var newMilitia = new Militia
        {
            Id = newMilitiaId,
            FullName = request.FullName,
            Email = request.Email,
            Type = request.Type,
            Rank = request.Rank,
            JoinDate = request.JoinDate,
            ManagerId = request.ManagerId,
        };

        await _militiaRepository.AddAsync(newMilitia);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newMilitia.Id;
    }
}