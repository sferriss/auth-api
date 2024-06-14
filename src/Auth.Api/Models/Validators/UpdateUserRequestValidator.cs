using Auth.Api.Models.Requests;
using FluentValidation;

namespace Auth.Api.Models.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .When(x => x.Name != null);
        
        RuleFor(x => x.Email)
            .MaximumLength(100)
            .EmailAddress()
            .When(x => x.Email != null);
        
        RuleFor(x => x.Login)
            .MinimumLength(4)
            .MaximumLength(50)
            .When(x => x.Login != null);
        
        RuleFor(x => x.Password)
            .MinimumLength(8)
            .MaximumLength(50)
            .When(x => x.Password != null);
        
        RuleFor(x => x.Contact)
            .SetValidator(new UserContactRequestValidator()!)
            .When(x => x.Contact != null);
    }
}