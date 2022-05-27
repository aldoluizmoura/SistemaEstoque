using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IProdutoService
    {
        Task<bool> AdicionarProduto(Produto produto);        
        Task<bool> AtualizarProduto(Produto produto);
        Task DebitarEstoque(Guid ProdutoId, int quantidade);
        Task ReporEstoque(Guid ProdutoId, int quantidade);
    }
}
