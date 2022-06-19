using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IProdutoService
    {
        Task<bool> AdicionarProduto(Produto produto);        
        Task<bool> AtualizarProduto(Produto produto);
        Task<bool> DebitarEstoque(Guid ProdutoId, int quantidade);
        Task<bool> ReporEstoque(Guid ProdutoId, int quantidade);
        Task MudarStatusProduto(Produto produto);
    }
}
