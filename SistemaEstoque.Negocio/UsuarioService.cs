using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.Negocio
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificador _notificador;

        public UsuarioService(IUsuarioRepository usuarioRepository, INotificador notificador)
        {
            _usuarioRepository = usuarioRepository;
            _notificador = notificador;
        }

        public async Task MudarStatusUsuario(Usuario usuario)
        {
            try
            {
                usuario.Ativo = usuario.Ativo ? usuario.Desativar() : usuario.Ativar();
                await _usuarioRepository.AtualizarUsuario(usuario);
            }
            catch (DbUpdateException ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Não foi possível atualizar {ex.Message}"));
            }
            catch (Exception ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Não foi possível atualizar {ex.Message}"));
            }
        }

        public async Task<bool> VerficarDisponibilidadeEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _notificador.AdicionarNotificacao(new Notificacao("email não pode ser vazio"));
                return false;
            }

            if (await _usuarioRepository.ObterPorEmail(email) is null)
                return true;

            _notificador.AdicionarNotificacao(new Notificacao("Email já está em uso"));
            return false;
        }

        public bool VerficarIdadeUsuario(DateTime dataNascimento)
        {
            var idade = DateTime.UtcNow.Year - dataNascimento.Year;

            if (idade >= 18) 
                return true;

            _notificador.AdicionarNotificacao(new Notificacao("Não é possivel cadastrar usuário menor de idade!"));
            return false;
        }
    }
}
