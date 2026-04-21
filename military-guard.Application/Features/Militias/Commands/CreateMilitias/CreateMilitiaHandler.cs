using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System.Globalization;
using System.Text;

namespace military_guard.Application.Features.Militias.Commands.CreateMilitias;

public class CreateMilitiaCommandHandler : IRequestHandler<CreateMilitiaCommand, Guid>
{
    private readonly IMilitiaRepository _militiaRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMilitiaCommandHandler(
        IMilitiaRepository militiaRepository,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _militiaRepository = militiaRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateMilitiaCommand request, CancellationToken cancellationToken)
    {
        bool isUnique = await _militiaRepository.IsEmailUniqueAsync(request.Email);
        if (!isUnique) throw new Exception($"Email {request.Email} đã tồn tại trong hệ thống.");

        var newMilitiaId = Guid.NewGuid();

        if (request.ManagerId.HasValue)
        {
            bool isValidManager = await _militiaRepository.IsValidManagerAsync(request.ManagerId.Value, newMilitiaId);
            if (!isValidManager) throw new Exception("Chỉ huy được chọn không tồn tại hoặc đã bị xóa khỏi hệ thống.");
        }

        string baseUsername = GenerateUsernameFromFullName(request.FullName);
        string finalUsername = baseUsername;
        int counter = 1;

        while (!await _accountRepository.IsUsernameUniqueAsync(finalUsername))
        {
            finalUsername = $"{baseUsername}{counter}";
            counter++;
        }

        var defaultRole = SystemRole.Militia;
        if (request.Rank == MilitiaRank.Commander || request.Rank == MilitiaRank.ViceCommander)
        {
            defaultRole = SystemRole.Commander;
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

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Username = finalUsername,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"),
            Role = defaultRole,
            MilitiaId = newMilitiaId
        };

        await _militiaRepository.AddAsync(newMilitia);
        await _accountRepository.AddAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newMilitia.Id;
    }

    private string GenerateUsernameFromFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return "user";

        string normalized = RemoveDiacritics(fullName).ToLower().Trim();
        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ");

        var words = normalized.Split(' ');
        if (words.Length == 1) return words[0];

        string lastName = words.Last();
        string initials = string.Join("", words.Take(words.Length - 1).Select(w => w[0]));

        return lastName + initials;
    }

    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Replace("đ", "d").Replace("Đ", "D");
    }
}