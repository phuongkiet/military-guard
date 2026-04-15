using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Employees.Commands.DeleteMilitias
{
    public record DeleteMilitiaCommand(Guid Id) : IRequest<Unit>;
}
