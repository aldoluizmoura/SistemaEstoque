using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IProdutoService
    {
        Task AdicionarProduto(Produto produto);        
        Task AtualizarProduto(Produto produto);
        Task DebitarEstoque(Guid ProdutoId, int quantidade);
        Task ReporEstoque(Guid ProdutoId, int quantidade);
        Task MudarStatusProduto(Produto produto);
    }
}
