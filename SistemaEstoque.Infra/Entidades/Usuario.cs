using Microsoft.AspNetCore.Identity;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Entidades.Extensions;
using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Exceptions;

namespace SistemaEstoque.Infra.Entidades
{
    public class Usuario : IdentityUser<Guid>
    {
        internal List<string> _errors;
        public IReadOnlyCollection<string> Errors => _errors;

        private string _nome = string.Empty;
        public string Nome
        {
            get { return _nome; }
            set { _nome = StringExtensions.CapitalizarString(value); }
        }

        public int Matricula { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public string Telefone { get; private set; }
        public bool Ativo { get; set; }

        //EF Relations
        public ICollection<Endereco> Enderecos { get; set; }
        public ICollection<Produto> Produtos { get; set; }
        public ICollection<Fabricante> Fabricantes { get; set; }
        public Documento Documento { get; private set; }
        public Guid DocumentoId { get; private set; }
        public Guid FabricanteId { get; private set; }

        protected Usuario() { }
        public Usuario(string nome, int matricula, Documento documento,
                        DateTime dataNascimento,
                        string telefone, string email)
        {
            Nome = nome;
            Matricula = matricula;
            Documento = documento;
            DataNascimento = dataNascimento;
            DataCriacao = DateTime.UtcNow;
            Ativo = true;
            Telefone = telefone;
            Email = email;
            UserName = email;

            Validar(documento);
        }

        public bool Ativar() => Ativo = true;
        public bool Desativar() => Ativo = false;

        private void Validar(Documento documento)
        {
            var validador = new UsuarioValidation();
            var validacao = validador.Validate(this);

            if (Documento.DefinirTipoDocumento(documento.Numero) == Infra.Enums.TipoDocumento.CNPJ)
                throw new EntidadeExcepetions("Para Usuários não é permitido esse tipo de documento");

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
