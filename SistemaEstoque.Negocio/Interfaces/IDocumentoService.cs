using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IDocumentoService
    {
        Task AdicionarDocumento(Documento documento);
        Task<bool> AtualizarDocumento(Documento documento);
        Task ExcluirDocumento(Documento documento);
        Task<bool> VerificarDisponibilidadeDocumento(string numeroDocumento);
    }
}
