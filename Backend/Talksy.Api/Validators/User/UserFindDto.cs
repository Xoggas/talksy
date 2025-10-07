using FluentValidation;
using Talksy.Api.Dtos.User;

namespace Talksy.Api.Validators.User;

public sealed class UserFindDtoValidator : AbstractValidator<UserFindDto>
{
    public UserFindDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Username exceeds 50 characters")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can contain only letters, digits, and underscore");
    }
}