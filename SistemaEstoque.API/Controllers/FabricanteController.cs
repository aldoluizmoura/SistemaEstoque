using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaEstoque.API.Autenticação;
using SistemaEstoque.API.Models;
using SistemaEstoque.API.Models.ModelsResponse;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;

namespace SistemaEstoque.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FabricanteController : ControllerBase
    {
        private readonly IFabricanteRepository _fabricanteRepository;
        private readonly IFabricanteService _fabricanteService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IDocumentoService _documentoService;
        private readonly IDocumentoRepository _documentoRepository;
        private readonly INotificador _notificador;        

        public FabricanteController(IFabricanteRepository fabricanteRepository,
                                    IMapper mapper,
                                    IUsuarioRepository usuarioRepository,
                                    IFabricanteService fabricanteService,
                                    IDocumentoService documentoService,
                                    IDocumentoRepository documentoRepository,
                                    INotificador notificador)
        {
            _fabricanteRepository = fabricanteRepository;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _fabricanteService = fabricanteService;
            _documentoService = documentoService;
            _notificador = notificador;
            _documentoRepository = documentoRepository;
        }

        [HttpGet("listar-fabricantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarFabricantes()
        {
            var fabricantes = await _fabricanteRepository.ObterFabricantes();           

            var listaFabricantes = fabricantes.Select(f => new FabricantesResponse(f.Nome, f.Documento.Numero, f.Codigo, f.Ativo, f.Id)).ToList();

            return Ok(listaFabricantes.OrderBy(f=>f.Codigo));
        }

        [HttpGet("pegar-fabricante-por-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PegarFabricantePorId(Guid fabricanteId)
        {
            var fabricante = await PegarFabricante(fabricanteId);

            if(fabricante is null)
                return NotFound("Fabricante não encontrado");
            
            var fabricanteResponse = new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id);
            return Ok(fabricanteResponse);
        }

        [ClaimsAuthorize("Fabricante", "Incluir")]
        [HttpPost("adicionar-fabricantes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AdicionarFabricantes(FabricanteDTO fabricanteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            await ValidarCadastro(fabricanteDto);

            try
            {
                await PegarUsuarioId(fabricanteDto);

                var fabricante = _mapper.Map<Fabricante>(fabricanteDto);
                fabricante.Documento = new Documento(fabricanteDto.NumeroDocumento);

                await _fabricanteService.AdicionarFabricante(fabricante);

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));                        

                return CreatedAtAction(nameof(AdicionarFabricantes), 
                    new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ClaimsAuthorize("Fabricante", "Editar")]
        [HttpPut("atualizar-fabricante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AtualizarFabricante(Guid fabricanteId, FabricanteDTO fabricanteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fabricante = await PegarFabricante(fabricanteId);
            if (fabricante is null)
                return NotFound("Fabricante não encontrado");

            _mapper.Map(fabricanteDto, fabricante);

            if (_notificador.TemNotificacao())
                return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));
            
            try
            {
                await _fabricanteService.AtualizarFabricante(fabricante);

                return Ok(new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ClaimsAuthorize("Fabricante", "Editar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("mudar-status-fabricante")]
        public async Task<IActionResult> MudarStatusFabricante([Required] Guid fabricanteId)
        {
            var fabricante = await PegarFabricante(fabricanteId);

            if (fabricante is null) 
                return NotFound("Fabricante não encontrado");

            try
            {
                await _fabricanteService.MudarStatusFabricante(fabricante);

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                return Ok(new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    return BadRequest(sqlException.Message);
                }

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ClaimsAuthorize("Fabricante", "Editar")]
        [HttpPut("alterar-nome-fabricante")]
        public async Task<IActionResult> AlteraNomeFabricante([Required] Guid fabricanteId, string nomeFabricante)
        {
            var fabricante = await PegarFabricante(fabricanteId);

            if (fabricante is null) 
                return NotFound("Fabricante não encontrado!");

            try
            {
                fabricante.AlterarNome(nomeFabricante);
                await _fabricanteService.AtualizarFabricante(fabricante);

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                return Ok(new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    return BadRequest(sqlException.Message);
                }

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [ClaimsAuthorize("Fabricante", "Editar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("alterar-documento-fabricante")]
        public async Task<IActionResult> AlterarDocumentoFabricante([Required] Guid fabricanteId, Guid documentoId)
        {
            var fabricante = await PegarFabricante(fabricanteId);
            if (fabricante is null)
                return NotFound("Fabricante não encontrado");           

            try
            {
                if (!await _documentoService.VerificarDisponibilidadeDocumento(fabricante.Documento.Numero))
                    return BadRequest("Documento não está mais disponível");

                await _fabricanteService.AlterarDocumentoFabricante(fabricanteId, documentoId);

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                return Ok(new FabricantesResponse(fabricante.Nome, fabricante.Documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    return BadRequest(sqlException.Message);
                }

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task ValidarCadastro(FabricanteDTO fabricanteDTO)
        {
            await _documentoService.VerificarDisponibilidadeDocumento(fabricanteDTO.NumeroDocumento);
        }        

        private async Task<Fabricante> PegarFabricante(Guid fabricanteId)
        {
            return await _fabricanteRepository.ObterPorId(fabricanteId);
        }

        private async Task PegarUsuarioId(FabricanteDTO fabricanteDto)
        {
            var context = new HttpContextAccessor().HttpContext;
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity.IsAuthenticated)
            {
                var claim = AuthExtension.PegarUsuarioIdDoContext(context);
                fabricanteDto.UsuarioId = new Guid(claim?.Value);
            }
        }
    }
}
