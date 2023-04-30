using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.Negocio
{
    public class CategoriaService : ICategoriaService
    {
        public ICategoriaRepository _categoriaRepository;
        private readonly INotificador _notificador;

        public CategoriaService(ICategoriaRepository categoriaRepository, INotificador notificador)
        {
            _categoriaRepository = categoriaRepository;
            _notificador = notificador;
        }

        public async Task AdicionarCategoria(Categoria categoria)
        {
            try
            {
                await _categoriaRepository.Adicionar(categoria);
            }
            catch (Exception ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Não foi possível adicionar a Categoria! {ex.Message}"));
            }
        }

        public async Task AlterarDescricaoCategoria(string descricaoCategoria, Guid categoriaId)
        {
            try
            {
                var categoria = await _categoriaRepository.ObterPorId(categoriaId);

                if (categoria is not null)
                {
                    categoria.Nome = descricaoCategoria;
                    await _categoriaRepository.Atualizar(categoria);
                }

                _notificador.AdicionarNotificacao(new Notificacao("Categoria não encontrada"));

            }
            catch (Exception ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Atualização não foi bem sucessedida! {ex.Message}"));
            }
        }
    }
}
