using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IFabricanteService 
    {
        Task AdicionarFabricante(Fabricante fabricante);        
        Task AtualizarFabricante(Fabricante fabricante);
        Task MudarStatusFabricante(Fabricante fabricante);
        Task AlterarDocumentoFabricante(Guid fabricanteId, Guid documentoId);
    }
}
