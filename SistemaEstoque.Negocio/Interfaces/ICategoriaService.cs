using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface ICategoriaService
    {
        Task<bool> AdicionarCategoria(Categoria categoria);        
        Task<bool> AtualizarCategoria(Categoria categoria);
    }
}
