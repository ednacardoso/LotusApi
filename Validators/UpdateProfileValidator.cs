using FluentValidation;
using Lotus.Models.DTOs.Requests;

namespace Lotus.Validators
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Telefone)
                .Matches(@"^\d{10,11}$")
                .When(x => !string.IsNullOrEmpty(x.Telefone))
                .WithMessage("Telefone deve conter 10 ou 11 dígitos numéricos");

            RuleFor(x => x.DataNascimento)
                .LessThan(DateTime.Now)
                .When(x => x.DataNascimento.HasValue)
                .WithMessage("Data de nascimento não pode ser futura");
        }
    }
}

