using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> ObterCategorias();
        Task<Categoria> ObterPorNome(string nomeCategoria);
    }
}
