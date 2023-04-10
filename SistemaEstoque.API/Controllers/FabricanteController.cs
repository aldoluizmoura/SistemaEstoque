using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.API.Models;
using SistemaEstoque.API.Models.ListasModels;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;
using System.Globalization;
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
            _documentoRepository= documentoRepository;
        }

        [HttpGet("listar-fabricantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ListarFabricantes()
        {
            var fabricantes = _mapper.Map<IEnumerable<FabricanteDTO>>(await _fabricanteRepository.ObterFabricantes());
            var listaFabricantes = new List<FabricantesModel>();

            foreach (var item in fabricantes)
            {
                var fabricante = await PegarFabricante(item.Id);
                var documento = await PegarDocumento(fabricante.DocumentoId);
                var fabricanteResponse = new FabricantesModel(item.Nome, documento.Numero);
                listaFabricantes.Add(fabricanteResponse);
            }

            return Ok(listaFabricantes);
        }

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
                CapitalizarNome(fabricanteDto);

                await PegarUsuarioPorId(fabricanteDto);

                var fabricante = _mapper.Map<Fabricante>(fabricanteDto);

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                await _fabricanteService.AdicionarFabricante(fabricante);

                return CreatedAtAction(nameof(AdicionarFabricantes), new { fabricante.Nome, fabricante.Documento.Numero });
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

        [HttpPut("atualizar-fabricante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AtualizarFabricante(Guid fabricanteId)
        {
            var fabricante = await PegarFabricante(fabricanteId);

            if (fabricante is null)
                return BadRequest("Fabricante não encontrado");

            try
            {
                await _fabricanteService.AtualizarFabricante(fabricante);
                return Ok(await PegarFabricante(fabricanteId));
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

        private async Task ValidarCadastro(FabricanteDTO fabricanteDTO)
        {
            await _documentoService.VerificarDisponibilidadeDocumento(fabricanteDTO.Documento.Numero);
        }

        private string CapitalizarNome(FabricanteDTO fabricanteDTO)
        {
            fabricanteDTO.Nome = CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(fabricanteDTO.Nome);
            return fabricanteDTO.Nome;
        }

        private async Task<Fabricante> PegarFabricante(Guid fabricanteId)
        {
            return await _fabricanteRepository.ObterPorId(fabricanteId);
        }

        private async Task PegarUsuarioPorId(FabricanteDTO fabricanteDto) 
        {            
            if(await _usuarioRepository.ObterPorId(fabricanteDto.UsuarioId) is null)
            {
                // pegar usuario via token
            }
        }

        private async Task<Documento> PegarDocumento(Guid documentoId)
        {
            return await _documentoRepository.ObterPorId(documentoId);
        }
    }
}
