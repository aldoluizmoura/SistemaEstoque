using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;

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

        public async Task DebitarEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            if(produto.QuantidadeEstoque > quantidade)
            {
                produto.DebitarEstoque(quantidade);
            }            
        }

        public async Task ReporEstoque(Guid ProdutoId, int quantidade)
        {
            var produto = await PegarProduto(ProdutoId);

            if (produto.QuantidadeEstoque > 0)
            {
                produto.ReporEstoque(quantidade);
            }
        }

        private async Task<Produto> PegarProduto(Guid ProdutoId)
        {
            return await _produtoRepository.ObterPorId(ProdutoId); ;
        }
    }
}
