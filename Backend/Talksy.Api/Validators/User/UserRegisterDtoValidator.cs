using FluentValidation;
using Talksy.Api.Dtos.User;

namespace Talksy.Api.Validators.User;

public sealed class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Username exceeds 50 characters")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can contain only letters, digits, and underscore");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Name exceeds 50 characters")
            .Matches(@"^[\p{L}\p{Nd} ]*$").WithMessage("Name can contain only letters and digits");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(50).WithMessage("Password exceeds 50 characters")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Password can contain only letters and digits")
            .Matches("[A-Z]").WithMessage("Password must have at least one capital letter")
            .Matches("[0-9]").WithMessage("Password must have at least one number");
    }
}