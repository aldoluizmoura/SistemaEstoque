using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using System.Net.Mime;

namespace SistemaEstoque.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class FabricanteController : ControllerBase
    {
        private readonly IFabricanteRepository _fabricanteRepository;
        private readonly IFabricanteService _fabricanteService;
        private readonly IMapper _mapper;
        public FabricanteController(IFabricanteRepository fabricanteRepository, 
                                    IMapper mapper, 
                                    IFabricanteService fabricanteService)
        {
            _fabricanteRepository = fabricanteRepository;
            _mapper = mapper;
            _fabricanteService = fabricanteService;
        }

        [HttpGet("listar-fabricantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _fabricanteRepository.ObterFabricantes();
            return Ok(produtos);
        }

        [HttpPost("adicionar-fabricantes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarFabricantes(FabricanteDto fabricanteDto)
        {
            var fabricante = _mapper.Map<Fabricante>(fabricanteDto);

            try
            {
                await _fabricanteService.AdicionarFabricante(fabricante);
                return CreatedAtAction(nameof(AdicionarFabricantes), new { fabricante.Nome, fabricante.Documento });
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
    }
}
