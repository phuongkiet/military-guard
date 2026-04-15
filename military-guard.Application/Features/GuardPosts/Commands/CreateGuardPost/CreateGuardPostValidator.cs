using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.CreateGuardPost
{
    public class CreateGuardPostValidator : AbstractValidator<CreateGuardPostCommand>
    {
        public CreateGuardPostValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.MinPersonnel).GreaterThan(0);
            RuleFor(x => x.MaxPersonnel).GreaterThanOrEqualTo(x => x.MinPersonnel);
        }
    }
}
