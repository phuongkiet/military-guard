using FluentValidation;
using military_guard.Application.Features.GuardPosts.Commands.CreateGuardPost;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.GuardPosts.Commands.UpdateGuardPost
{
    public class UpdateGuardPostValidator : AbstractValidator<UpdateGuardPostCommand>
    {
        public UpdateGuardPostValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.MinPersonnel).GreaterThan(0).WithMessage("Số người tối thiểu phải lớn hơn 0.");
            RuleFor(x => x.MaxPersonnel).GreaterThanOrEqualTo(x => x.MinPersonnel).WithMessage("Số người tối đa không được nhỏ hơn số người tối thiểu.");
        }
    }
}
