using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.API.Models;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _produtoRepository.ObterProdutos();
            return Ok(produtos);
        }

        [HttpGet("pegar-produto-por-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PegarProdutoPorId(Guid ProdutoId)
        {
            var produto = await _produtoRepository.ObterPorId(ProdutoId);
            return Ok(produto);
        }

        [HttpGet("obter-produtos-por-categoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ObterProdutosCategoria(Guid CategoriaId)
        {
            var produtos = await _produtoRepository.ObterProdutosPorCategorias(CategoriaId);
            return Ok(produtos);
        }

        [HttpGet("listar-categorias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarCategorias()
        {
            var categorias = await _categoriaRepository.ObterCategorias();
            return Ok(categorias);
        }

        [HttpPost("adicionar-categoria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarCategoria(CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            try
            {
                await _categoriaService.AdicionarCategoria(categoria);
                return CreatedAtAction(nameof(AdicionarCategoria), new { categoria.Nome });
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("atualizar-categoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AtualizarCategoria(Guid categoriaId)
        {
            var categoria = await PegarCategoria(categoriaId);

            if (categoria is null) 
                return BadRequest("Categoria não encontrada");

            try
            {
                await _categoriaService.AtualizarCategoria(categoria);
                return Ok(await PegarCategoria(categoriaId));
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("adicionar-produtos")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarProdutos(ProdutoDto produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            try
            {
                await _produtoService.AdicionarProduto(produto);
                return CreatedAtAction(nameof(AdicionarProdutos), new { produto.Modelo });
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("alterar-categoria-produto")]
        public async Task<IActionResult> AlterarCategoriaProduto([Required] Guid produtoId, string nomeCategoria)
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
        public async Task<IActionResult> AlteraDescricaoProduto([Required] Guid produtoId, string descricaoProduto)
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
        public async Task<IActionResult> MudarStatusProduto([Required] Guid produtoId)
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
        public async Task<IActionResult> AlterarProduto([Required] Guid produtoId, ProdutoDto produtoDto)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return BadRequest("Produto não encontrado");

            try
            {
                await _produtoService.AtualizarProduto(produto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("repor-estoque")]
        public async Task<IActionResult> ReporEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) return BadRequest("Produto não encontrado");

            try
            {
                if(await _produtoService.ReporEstoque(produto.Id, quantidade)) 
                    return Ok(await PegarProduto(produtoId));

                return BadRequest("Quantidade inválida");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("debitar-estoque")]
        public async Task<IActionResult> DebitarEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return BadRequest("Produto não encontrado");

            try
            {
                if(await _produtoService.DebitarEstoque(produto.Id, quantidade)) 
                    return Ok(await PegarProduto(produtoId));

                return BadRequest(new ProdutoErrorInfo(produto.QuantidadeEstoque,quantidade));
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message); 
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
