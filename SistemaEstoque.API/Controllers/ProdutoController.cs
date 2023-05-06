using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.API.Autenticação;
using SistemaEstoque.API.Models;
using SistemaEstoque.API.Models.ModelsResponse;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mime;
using System.Security.Claims;

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
            
            var listaProdutos = produtos.Select(p => new ProdutoResponse(p.Descricao, p.Codigo, p.Ativo, p.DataVencimento, p.Marca, p.Modelo, p.Categoria.Nome, p.QuantidadeEstoque, p.Id))
                                        .OrderBy(p => p.Codigo).ToList();

            return Ok(listaProdutos.OrderBy(p => p.Codigo));
        }

        [HttpGet("pegar-produto-por-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PegarProdutoPorId(Guid ProdutoId)
        {
            var produto = await _produtoRepository.ObterPorId(ProdutoId);
            if(produto is null)
                return NotFound("Produto não encontrado!");

            return Ok(new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id));
        }

        [HttpGet("obter-produtos-por-categoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ObterProdutosCategoria(Guid CategoriaId)
        {
            var produtos = await _produtoRepository.ObterProdutosPorCategorias(CategoriaId);                       

            var listaProdutos = produtos.Select(async item =>
            {
                var produto = await PegarProduto(item.Id);
                return new ProdutoResponse(
                    item.Descricao,
                    item.Codigo,
                    item.Ativo,
                    item.DataVencimento,
                    item.Marca,
                    item.Modelo,
                    item.Categoria.Nome,
                    produto.QuantidadeEstoque,
                    produto.Id);
            })
            .OrderBy(p => p.Result.NomeCategoria)
            .ToList();

            if (listaProdutos.Count == 0)
                return NotFound("Nada encontrado!");

            return Ok(listaProdutos);
        }

        [HttpGet("listar-categorias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarCategorias()
        {
            var categorias = await _categoriaRepository.ObterCategorias();           

            var listaCategorias = categorias.Select(c => new CategoriasResponse(c.Nome, c.Codigo, c.Id));

            if (!listaCategorias.Any())
                return NotFound("Nenhuma Categoria cadastrada!");

            return Ok(listaCategorias);
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
                var categoriaResponse = new CategoriasResponse(categoria.Nome, categoria.Codigo, categoria.Id);
                return CreatedAtAction(nameof(AdicionarCategoria), categoriaResponse);
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("alterar-descricao-categoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AlterarDescricaoCategoria(Guid categoriaId, string descricaoCategoria)
        {
            try
            {
                await _categoriaService.AlterarDescricaoCategoria(descricaoCategoria, categoriaId);
                var categoria = await PegarCategoria(categoriaId);
                var categoriaResponse = new CategoriasResponse(categoria.Nome, categoria.Codigo, categoria.Id);
                return CreatedAtAction(nameof(AdicionarCategoria), categoriaResponse);
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("adicionar-produtos")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarProdutos(ProdutoDto produtoDto)
        {
            try
            {
                PegarUsuarioId(produtoDto);

                var produto = _mapper.Map<Produto>(produtoDto);

                var categoria = await _categoriaRepository.ObterPorId(produtoDto.CategoriaId);
                if(categoria is null)
                    return NotFound("Categoria não encontrada!");               

                await _produtoService.AdicionarProduto(produto);
                var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                return CreatedAtAction(nameof(AdicionarProdutos), produtoResponse);
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("alterar-categoria-produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AlterarCategoriaProduto([Required] Guid produtoId, string nomeCategoria)
        {
            var produto = await PegarProduto(produtoId);
            var categoria = await _categoriaRepository.ObterPorNome(nomeCategoria);

            if (produto is null) 
                return NotFound("Produto não encontrado");

            try
            {
                produto.AlterarCategoria(categoria);
                var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                return CreatedAtAction(nameof(AdicionarProdutos), produtoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("alterar-descricao-produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AlteraDescricaoProduto([Required] Guid produtoId, string descricaoProduto)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return NotFound("Produto não encontrado");

            try
            {
                produto.AlterarDescricao(descricaoProduto);
                var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                return CreatedAtAction(nameof(AdicionarProdutos), produtoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("mudar-status-produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> MudarStatusProduto([Required] Guid produtoId)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return NotFound("Produto não encontrado");

            try
            {
                await _produtoService.MudarStatusProduto(produto);
                var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                return CreatedAtAction(nameof(MudarStatusProduto), produtoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("alterar-produto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AlterarProduto([Required] Guid produtoId, ProdutoDto produtoDto)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return BadRequest("Produto não encontrado");

            try
            {
                produto = _mapper.Map<Produto>(produtoDto);
                await _produtoService.AtualizarProduto(produto);
                return Ok(new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("repor-estoque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ReporEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null)
                return NotFound("Produto não encontrado");

            try
            {
                if(await _produtoService.ReporEstoque(produto.Id, quantidade))
                {
                    var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                    return CreatedAtAction(nameof(ReporEstoque), produtoResponse);
                }                    

                return BadRequest("Quantidade inválida");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("debitar-estoque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DebitarEstoque([Required] Guid produtoId, [Required] int quantidade)
        {
            var produto = await PegarProduto(produtoId);

            if (produto is null) 
                return NotFound("Produto não encontrado");

            try
            {
                if(await _produtoService.DebitarEstoque(produto.Id, quantidade))
                {
                    var produtoResponse = new ProdutoResponse(produto.Descricao, produto.Codigo, produto.Ativo, produto.DataVencimento, produto.Marca, produto.Modelo, produto.Categoria.Nome, produto.QuantidadeEstoque, produto.Id);
                    return CreatedAtAction(nameof(DebitarEstoque), produtoResponse);
                }                

                return BadRequest(new ProdutoErrorInfo(produto.QuantidadeEstoque,quantidade));
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
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

        private async Task PegarUsuarioId(ProdutoDto produtoDto)
        {
            var context = new HttpContextAccessor().HttpContext;
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity.IsAuthenticated)
            {
                var claim = AuthExtension.PegarUsuarioIdDoContext(context);
                produtoDto.UsuarioId = new Guid(claim?.Value);
            }
        }

        private static string CapitalizarString(string value)
        {
            return CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(value);
        }
    }
}
