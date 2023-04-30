﻿using AutoMapper;
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
using SistemaEstoque.Negocio.Notificacões;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
            var fabricantes = _mapper.Map<IEnumerable<FabricanteDTO>>(await _fabricanteRepository.ObterFabricantes());
            var listaFabricantes = new List<FabricantesResponse>();

            if (fabricantes.Any())
            {
                foreach (var item in fabricantes)
                {
                    var fabricante = await PegarFabricante(item.Id);
                    var documento = await PegarDocumento(fabricante.DocumentoId);
                    var fabricanteResponse = new FabricantesResponse(item.Nome, documento.Numero, item.Codigo, item.Ativo, item.Id);
                    listaFabricantes.Add(fabricanteResponse);
                }
            }

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

            var documento = await PegarDocumento(fabricante.DocumentoId);
            var fabricanteResponse = new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id);
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
                fabricanteDto.Nome = CapitalizarNome(fabricanteDto.Nome);

                await PegarUsuarioId(fabricanteDto);

                var fabricante = _mapper.Map<Fabricante>(fabricanteDto);                                        

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                await _fabricanteService.AdicionarFabricante(fabricante);
                var documento = await PegarDocumento(fabricante.DocumentoId);

                return CreatedAtAction(nameof(AdicionarFabricantes), new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
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

        [ClaimsAuthorize("Fabricante", "Editar")]
        [HttpPut("atualizar-fabricante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AtualizarFabricante(Guid fabricanteId, FabricanteDTO fabricanteDto)
        {
            var fabricante = _mapper.Map<Fabricante>(fabricanteDto);

            if (fabricante is null)
                return NotFound("Fabricante não encontrado");

            try
            {
                await _fabricanteService.AtualizarFabricante(fabricante);
                var documento = await PegarDocumento(fabricante.DocumentoId);
                return Ok(new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
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
                var documento = await PegarDocumento(fabricante.DocumentoId);
                return Ok(new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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

                var documento = await PegarDocumento(fabricante.DocumentoId);
                return Ok(new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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

            var documento = await PegarDocumento(documentoId);
            if (documento is null)
                return NotFound("Documento não existe");

            try
            {
                if (!await _documentoService.VerificarDisponibilidadeDocumento(documento.Numero))
                    return BadRequest("Documento não está mais disponível");

                await _fabricanteService.AlterarDocumentoFabricante(fabricanteId, documentoId);

                return Ok(new FabricantesResponse(fabricante.Nome, documento.Numero, fabricante.Codigo, fabricante.Ativo, fabricante.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task ValidarCadastro(FabricanteDTO fabricanteDTO)
        {
            await _documentoService.VerificarDisponibilidadeDocumento(fabricanteDTO.Documento.NumeroDocumento);
        }

        private static string CapitalizarNome(string nome)
        {
            return CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(nome);
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

        private async Task<Documento> PegarDocumento(Guid documentoId)
        {
            return await _documentoRepository.ObterPorId(documentoId);
        }
    }
}
