using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.Negocio
{
    public class ProdutoService : IProdutoService
    {
        public IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> AdicionarProduto(Produto produto)
        {
            try
            {
                await _produtoRepository.Adicionar(produto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AtualizarProduto(Produto produto)
        {
            try
            {
                if (await _produtoRepository.ObterPorId(produto.Id) is not null)
                {
                    await _produtoRepository.Atualizar(produto);
                    return true;
                }

                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DebitarEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            if (produto.QuantidadeEstoque >= quantidade)
            {
                produto.DebitarEstoque(quantidade);
                await AtualizarProduto(produto);
                return true;
            }

            return false;
        }

        public async Task<bool> ReporEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            if (produto.QuantidadeEstoque > 0)
            {
                produto.ReporEstoque(quantidade);
                await AtualizarProduto(produto);
                return true;
            }

            return false;
        }

        public async Task MudarStatusProduto(Produto produto)
        {

            if (produto.Ativo)
            {
                produto.Desativar();
                await AtualizarProduto(produto);
            }

            produto.Ativar();
            await AtualizarProduto(produto);


        }

        private async Task<Produto> PegarProduto(Guid ProdutoId)
        {
            return await _produtoRepository.ObterPorId(ProdutoId); ;
        }
    }
}
