using SistemaEstoque.Infra.Entidades.Validações;

namespace SistemaEstoque.Infra.Entidades
{
    public class Fabricante : Entity
    {
        public string Nome { get; private set; }      
        public ICollection<Produto> Produtos { get; set; }

        // EF relations
        public Documento Documento { get; private set; }
        public Guid DocumentoId { get; private set; }       

        public Fabricante(){}

        public Fabricante(string nome, Documento documento)
        {
            Nome = nome;
            Documento = documento;
            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "Nome do Fabricante não pode ser vázio");            
        }
        
    }
}
