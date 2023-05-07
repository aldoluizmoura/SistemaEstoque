using SistemaEstoque.Infra.Entidades.Extensions;
using SistemaEstoque.Infra.Entidades.Validações;
using System.Globalization;

namespace SistemaEstoque.Infra.Entidades
{
    public class Categoria : Entity
    {
        private string _nome = string.Empty;
        public string Nome
        {
            get { return _nome; }
            set { _nome = StringExtensions.CapitalizarString(value); }
        }
        public int Codigo { get; private set; }
        public DateTime DataCriacao { get; set; }

        // EF relations
        public ICollection<Produto> Produtos { get; set; }
        public Categoria(){}

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;
            DataCriacao = DateTime.UtcNow;

            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome não pode ser vázio");
            Validacoes.ValidarSeIgual(Codigo, 0, "O campo Código não pode ser 0");
        }        
    }
}
