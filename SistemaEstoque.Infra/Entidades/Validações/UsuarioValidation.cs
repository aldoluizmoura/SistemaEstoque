using FluentValidation;

namespace SistemaEstoque.Infra.Entidades.Validações
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(x => x).NotEmpty().WithMessage("A entidade não pode ser vazia.")
           .NotNull().WithMessage("Entidade não pode ser nula");

            RuleFor(x => x.Nome).NotNull().WithMessage("O nome não pode ser vazio.")
            .NotEmpty().WithMessage("O nome não pode ser nulo")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
            .MaximumLength(80).WithMessage("O nome deve ter no máximo 80 caracteres");

            RuleFor(x => x.Email).NotNull().WithMessage("o e-mail não pode ser nulo.")
                .NotEmpty().WithMessage("o e-mail não pode ser vazio")
                .MinimumLength(10).WithMessage("o e-mail tem que ter no mínimo 10 caracteres")
                .MaximumLength(180).WithMessage("o e-mail só pode ter no máximo 180 caracteres")
            .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
            .WithMessage("e-mail não válido");
        }
    }
}
