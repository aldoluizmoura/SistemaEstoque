using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutos();        
        Task<IEnumerable<Produto>> ObterProdutosPorCategorias(Guid categoriaId);
    }
}
