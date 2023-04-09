using Microsoft.AspNetCore.Identity;
using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Exceptions;
using System.Globalization;

namespace SistemaEstoque.Infra.Entidades
{
    public class Usuario : IdentityUser
    {
        internal List<string> _errors;
        public IReadOnlyCollection<string> Errors => _errors;

        public Guid UsuarioId { get; set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Telefone { get; private set; }        
        public bool Ativo { get; set; }

        //EF Relations
        public ICollection<Endereco> Enderecos { get; set; }
        public ICollection<Produto> Produtos { get; set; }
        public Documento Documento { get; private set; }
        public Guid DocumentoId { get; private set; }

        protected Usuario() { }
        public Usuario(string nome, Documento documento, 
                        DateTime dataNascimento,
                        string telefone, string email)
        {
            UsuarioId = Guid.NewGuid();
            Nome = nome;
            Documento = documento;
            DataNascimento = dataNascimento;
            Ativo = true;
            Telefone = telefone;            
            Email = email;
            UserName = email;

            Validar();            
        }      

        public bool Ativar() => Ativo = true;
        public bool Desativar() => Ativo = false;

        private void Validar()
        {
            var validador = new UsuarioValidation();
            var validacao = validador.Validate(this);

            if (!validacao.IsValid)
            {
                foreach (var item in validacao.Errors)
                {
                    _errors.Add(item.ErrorMessage);
                    throw new EntidadeExcepetions($"{_errors}");
                }
            }
        }       
    }
}
