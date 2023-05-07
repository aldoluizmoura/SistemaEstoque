using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface IFabricanteRepository : IRepository<Fabricante>
    {
        Task<IEnumerable<Fabricante>> ObterFabricantes();
        Task<Fabricante> ObterPorNome(string nomeFabricante);
        Task<Fabricante> ObterPorId(Guid fabricanteId);
    }
}
