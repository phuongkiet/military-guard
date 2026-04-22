using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.HolidayEvents.Commands.CreateHolidayEvent
{
    public class CreateHolidayEventHandler : IRequestHandler<CreateHolidayEventCommand, Guid>
    {
        private readonly IHolidayEventRepository _holidayEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateHolidayEventHandler(IHolidayEventRepository holidayEventRepository,
            IUnitOfWork unitOfWork)
        {
            _holidayEventRepository = holidayEventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateHolidayEventCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _holidayEventRepository.IsHolidayEventUniqueAsync(request.Name, request.Date);
            if (!isUnique)
            {
                throw new Exception($"Ngày/Lễ {request.Name} đã tồn tại trong hệ thống.");
            }

            var newEventId = Guid.NewGuid();

            var newEvent = new HolidayEvent
            {
                Id = newEventId,
                Name = request.Name,
                Date = request.Date,
                RequiresMobileForces = request.RequiresMobileForces,
                Description = request.Description ?? null
            };

            await _holidayEventRepository.AddAsync(newEvent);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newEvent.Id;
        }
    }
}
