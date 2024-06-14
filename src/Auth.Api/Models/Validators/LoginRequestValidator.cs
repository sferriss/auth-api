using Auth.Api.Models.Requests;
using FluentValidation;

namespace Auth.Api.Models.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}