using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface IDocumentoRepository : IRepository<Documento>
    {
        Task<IEnumerable<Documento>> ObterDocumentos();
        Task<Documento> ObterPorNumero(string numeroDocumento);
    }
}
