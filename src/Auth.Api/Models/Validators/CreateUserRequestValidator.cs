using Auth.Api.Models.Requests;
using FluentValidation;

namespace Auth.Api.Models.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();
        
        RuleFor(x => x.Login)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(50);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50);
        
        RuleFor(x => x.Contact)
            .NotNull()
            .SetValidator(new UserContactRequestValidator());
    }
}