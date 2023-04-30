namespace SistemaEstoque.API.Models.ModelsResponse
{
    public class CategoriasResponse
    {
        public string Nome { get; set; } = string.Empty;
        public int Codigo { get; set; }
        public Guid Id { get; set; }

        public CategoriasResponse(string nome, int codigo, Guid id)
        {
            Nome = nome;
            Codigo = codigo;
            Id = id;
        }
    }
}
