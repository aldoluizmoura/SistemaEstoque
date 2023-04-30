using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface IUsuarioRepository 
    {
        Task<IEnumerable<Usuario>> ObterUsuarios();
        Task<IEnumerable<Usuario>> ObterPorNome(string nomeUsuario);
        Task<Usuario> ObterPorEmail(string emailUsuario);
        Task<Usuario> ObterPorDocumento(string documento);
        Task<Usuario> ObterPorId(Guid usuarioId);
        Task AtualizarUsuario(Usuario usuario);
    }
}
