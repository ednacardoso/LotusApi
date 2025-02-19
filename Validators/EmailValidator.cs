using FluentValidation;
using Lotus.Models.DTOs.Requests;

namespace Lotus.Validators
{
    public class EmailValidator : AbstractValidator<EmailRequest>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100)
                .WithMessage("Email inválido");
        }
    }
}
