using military_guard.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using military_guard.Application.Features.Militias.DTOs;

namespace military_guard.Application.Features.Militias.Queries.GetMilitiaById
{
    public class GetMilitiaByIdHandler : IRequestHandler<GetMilitiaByIdQuery, MilitiaResponse>
    {
        private readonly IMilitiaRepository _militiaRepository;

        public GetMilitiaByIdHandler(IMilitiaRepository militiaRepository)
        {
            _militiaRepository = militiaRepository;
        }

        public async Task<MilitiaResponse> Handle(GetMilitiaByIdQuery request, CancellationToken cancellationToken)
        {
            var militia = await _militiaRepository.GetMilitiaDetailsAsync(request.Id);

            if (militia == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy dân quân với ID: {request.Id}");
            }

            return new MilitiaResponse(
                Id: militia.Id,
                FullName: militia.FullName,
                Email: militia.Email,
                Type: militia.Type.ToString(),
                Rank: militia.Rank.ToString(),
                MonthsOfService: militia.MonthsOfService,
                ManagerName: militia.Manager?.FullName
            );
        }
    }
}
