using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.API.DTOs;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;        
        
        public ProdutoController(IProdutoRepository produtoRepository, 
                                 IProdutoService produtoService,
                                 IMapper mapper,
                                 ICategoriaService categoriaService,
                                 ICategoriaRepository categoriaRepository)
        {
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _categoriaService = categoriaService;
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet("listar-produtos")]
        public async Task<IActionResult> listarProdutos()
        {
            var produtos = await _produtoRepository.ObterProdutos();
            return Ok(produtos);
        }

        [HttpGet("pegar-produto-por-id")]
        public async Task<IActionResult> PegarProdutoPorId(Guid ProdutoId)
        {
            var produto = await _produtoRepository.ObterPorId(ProdutoId);
            return Ok(produto);
        }

        [HttpGet("obter-produtos-por-categoria")]
        public async Task<IActionResult> obterProdutosCategoria(Guid CategoriaId)
        {
            var produtos = await _produtoRepository.ObterProdutosPorCategorias(CategoriaId);
            return Ok(produtos);
        }

        [HttpGet("listar-categorias")]
        public async Task<IActionResult> listarCategorias()
        {
            var categorias = await _categoriaRepository.ObterCategorias();
            return Ok(categorias);
        }

        [HttpPost("adicionar-categoria")]
        public async Task<IActionResult> adicionarCategoria(CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            try
            {
                await _categoriaService.AdicionarCategoria(categoria);
                return Ok(categoriaDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("atualizar-categoria")]
        public async Task<IActionResult> atualizarCategoria(Guid categoriaId)
        {
            var categoria = await PegarCategoria(categoriaId);

            if (categoria is null) return BadRequest("Categoria não encontrada");

            try
            {
                await _categoriaService.AtualizarCategoria(categoria);
                return Ok(await PegarCategoria(categoriaId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("adicionar-produtos")]
        public async Task<IActionResult> adicionarProdutos(ProdutoDto produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            try
            {
                await _produtoService.AdicionarProduto(produto);
                return Ok(produto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("alterar-categoria-produto")]
        public async Task<IActionResult> alterarCategoriaProduto([Required] Guid produtoId, string nomeCategoria)
        {
            var produto = await PegarProduto(produtoId);
            var categoria = await _categoriaRepository.ObterPorNome(nomeCategoria);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                produto.AlterarCategoria(categoria);                
                return Ok(produto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("alterar-descricao-produto")]
        public async Task<IActionResult> alteraDescricaoProduto([Required] Guid produtoId, string descricaoProduto)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                produto.AlterarDescricao(descricaoProduto);
                return Ok(_mapper.Map<ProdutoDto>(produto));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("mudar-status-produto")]
        public async Task<IActionResult> mudarStatusProduto([Required] Guid produtoId)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                await _produtoService.MudarStatusProduto(produto);
                return Ok(await PegarProduto(produtoId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("alterar-produto")]
        public async Task<IActionResult> alterarProduto([Required] Guid produtoId, ProdutoDto produtoDto)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                await _produtoService.AtualizarProduto(produto);
                return Ok(produto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("repor-estoque")]
        public async Task<IActionResult> reporEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                if(await _produtoService.ReporEstoque(produto.Id, quantidade)) return Ok(await PegarProduto(produtoId));

                return BadRequest("Quantidade inválida");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("debitar-estoque")]
        public async Task<IActionResult> debitarEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                if(await _produtoService.DebitarEstoque(produto.Id, quantidade)) return Ok(await PegarProduto(produtoId));

                return BadRequest(new ProdutoErrorInfo(produto.QuantidadeEstoque,quantidade));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }       

        private async Task<Produto> PegarProduto(Guid produtoId)
        {
            return await _produtoRepository.ObterPorId(produtoId);
        }

        private async Task<Categoria> PegarCategoria(Guid categoriaId)
        {
            return await _categoriaRepository.ObterPorId(categoriaId);
        }
    }
}
