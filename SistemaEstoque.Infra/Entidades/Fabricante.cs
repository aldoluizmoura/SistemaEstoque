using SistemaEstoque.Infra.Entidades.Extensions;
using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Exceptions;

namespace SistemaEstoque.Infra.Entidades
{
    public class Fabricante : Entity
    {
        private string _nome = string.Empty;
        public string Nome
        {
            get { return _nome; }
            set { _nome = StringExtensions.CapitalizarString(value); }
        }
        public int Codigo { get; private set; }
        public DateTime DataCriacao { get; set; }
        public Usuario Usuario { get; set; }
        public Guid UsuarioId { get; private set; }        
        public Documento Documento { get; set; }
        public Guid DocumentoId { get; private set; }
        public bool Ativo { get; set; }

        public Fabricante(){}

        public Fabricante(string nome, int codigo, Documento documento, Guid usuarioId)
        {
            Nome = nome;
            Codigo = codigo;
            Documento = documento;
            UsuarioId = usuarioId;
            DataCriacao = DateTime.UtcNow;
            Ativo = true;

            Validar(documento);
        }

        public bool Ativar() => Ativo = true;
        public bool Desativar() => Ativo = false;

        public void AlterarNome(string nome)
        {
            Validacoes.ValidarSeVazio(nome, "O Nome não pode ser vazio");
            Nome = nome;
        }

        public void TrocarDocumentoFabricante(Guid documentoId) => DocumentoId = documentoId;

        private void Validar(Documento documento)
        {
            Validacoes.ValidarSeVazio(Nome, "Nome do Fabricante não pode ser vazio.");
            Validacoes.ValidarSeNulo(Documento, "Documento do Fabricante não pode ser nulo.");
            Validacoes.ValidarSeNulo(UsuarioId, "Fabricante precisa de um usuário associado.");

            if (Documento.DefinirTipoDocumento(documento.Numero) == Enums.TipoDocumento.CPF)
                throw new EntidadeExcepetions("Para Fabricantes não é permitido esse tipo de documento");
        }
    }
}
