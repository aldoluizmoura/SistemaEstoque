using SistemaEstoque.Infra.Entidades.Validações;

namespace SistemaEstoque.Infra.Entidades
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
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
