using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface ICategoriaService
    {
        Task AdicionarCategoria(Categoria categoria);        
        Task AlterarDescricaoCategoria(string descricaoCategoria, Guid categoriaId);
    }
}
