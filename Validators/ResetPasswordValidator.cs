using FluentValidation;
using Lotus.Models.DTOs.Requests;

namespace Lotus.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token é obrigatório");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
                .Matches("[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
                .Matches("[0-9]").WithMessage("Senha deve conter pelo menos um número")
                .Matches("[^a-zA-Z0-9]").WithMessage("Senha deve conter pelo menos um caractere especial");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("As senhas não conferem");
        }
    }
}

 