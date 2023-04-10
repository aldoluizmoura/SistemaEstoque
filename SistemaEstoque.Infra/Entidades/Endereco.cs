using SistemaEstoque.Infra.Entidades.Validações;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.Infra.Entidades
{
    public class Endereco : Entity
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime DataCriacao { get; set; }

        // ER Relations
        public Usuario Usuario { get; set; }
        public Guid UsuarioId { get; set; }

        protected Endereco(){}

        public Endereco(Guid usuarioId, string logradouro, 
                        string numero, string cep, string bairro,
                        string cidade, string estado)
        {
            UsuarioId = usuarioId;
            Logradouro = logradouro;
            Numero = numero;
            Cep = cep;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            DataCriacao = DateTime.UtcNow;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Logradouro, "Logradouro não pode ser vázio");
            Validacoes.ValidarSeVazio(Numero, "Numero não pode ser vázio");
            Validacoes.ValidarSeVazio(Cep, "Cep não pode ser vázio");
            Validacoes.ValidarSeVazio(Bairro, "Bairro não pode ser vázio");
            Validacoes.ValidarSeVazio(Cidade, "Cidade não pode ser vázio");
            Validacoes.ValidarSeVazio(Estado, "Estado não pode ser vázio");
        }
    }
}
