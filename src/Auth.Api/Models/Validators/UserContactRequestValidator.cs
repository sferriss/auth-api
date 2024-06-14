using Auth.Api.Models.Requests;
using FluentValidation;

namespace Auth.Api.Models.Validators;

public class UserContactRequestValidator : AbstractValidator<UserContactRequest>
{
    public UserContactRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MinimumLength(11)
            .MaximumLength(12)
            .Matches(@"^\d+$");
    }
}