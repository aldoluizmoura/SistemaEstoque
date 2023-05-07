using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.Negocio
{
    public class ProdutoService : IProdutoService
    {
        public IProdutoRepository _produtoRepository;
        private readonly INotificador _notificador;

        public ProdutoService(IProdutoRepository produtoRepository, INotificador notificador)
        {
            _produtoRepository = produtoRepository;
            _notificador = notificador;
        }

        public async Task AdicionarProduto(Produto produto)
        {
            try
            {
                await _produtoRepository.Adicionar(produto);
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

        public async Task AtualizarProduto(Produto produto)
        {
            try
            {
                if (await _produtoRepository.ObterPorId(produto.Id) is not null)
                {
                    await _produtoRepository.Atualizar(produto);
                }

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

        public async Task DebitarEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            try
            {
                if (produto.QuantidadeEstoque >= quantidade)
                {
                    produto.DebitarEstoque(quantidade);
                    await AtualizarProduto(produto);
                }
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

        public async Task ReporEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            try
            {
                if (produto.QuantidadeEstoque > 0)
                {
                    produto.ReporEstoque(quantidade);
                    await AtualizarProduto(produto);
                }
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

        public async Task MudarStatusProduto(Produto produto)
        {
            try
            {
                produto.Ativo = produto.Ativo ? produto.Desativar() : produto.Ativar();
                await AtualizarProduto(produto);
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

        private async Task<Produto> PegarProduto(Guid ProdutoId)
        {
            return await _produtoRepository.ObterPorId(ProdutoId); ;
        }
    }
}
