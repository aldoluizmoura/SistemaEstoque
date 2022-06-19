using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Negocio.Interfaces
{
    public interface IUsuarioService
    {
        Task MudarStatusUsuario(Usuario usuario);
        Task<bool> VerficarDisponibilidadeEmail(string email);
        bool VerficarIdadeUsuario(DateTime dataNascimento);        
    }
}
