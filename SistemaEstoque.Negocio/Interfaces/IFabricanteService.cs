using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IFabricanteService 
    {
        Task<bool> AdicionarFabricante(Fabricante fabricante);        
        Task<bool> AtualizarFabricante(Fabricante fabricante);
        Task MudarStatusFabricante(Fabricante fabricante);
        Task AlterarDocumentoFabricante(Guid fabricanteId, Guid documentoId);
    }
}
