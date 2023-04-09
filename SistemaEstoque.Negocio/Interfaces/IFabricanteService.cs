using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IFabricanteService 
    {
        Task<bool> AdicionarFabricante(Fabricante fabricante);        
        Task<bool> AtualizarFabricante(Fabricante fabricante);
    }
}
