using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoque.API.DTOs;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.API.Controllers
{
    [Route("api/conta-usuario")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;        
        private readonly INotificador _notificador;
        private readonly IDocumentoService _documentoService;
        private readonly IUsuarioService _usuarioService;
        
        public AuthController(SignInManager<Usuario> signInManager,
                               UserManager<Usuario> userManager, 
                               IMapper mapper, INotificador notificador,
                               IDocumentoService documentoService, 
                               IUsuarioService usuarioService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _notificador = notificador;
            _documentoService = documentoService;
            _usuarioService = usuarioService;
        }

        [HttpPost("cadastrar-usuario")]
        public async Task<IActionResult> CadastrarUsuario(RegistroUsuarioDTO registroUsuarioDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            await ValidarCadastro(registroUsuarioDTO);

            if (_notificador.TemNotificacao()) return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

            var usuario = _mapper.Map<Usuario>(registroUsuarioDTO);            

            var result = await _userManager.CreateAsync(usuario, registroUsuarioDTO.Senha);

            if (result.Succeeded)
            {
                return Ok("Usuário Cadastrado com Sucesso!");
            }

            var documento = PegarDocumento(registroUsuarioDTO.Documento);
            await _documentoService.ExcluirDocumento(documento);

            foreach (var item in result.Errors)
            {                
                var notificacao = new Notificacao(item.Description);
                _notificador.AdicionarNotificacao(notificacao);
            }

            return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));           
        }

        private async Task ValidarCadastro(RegistroUsuarioDTO registroUsuarioDTO)
        {
            await _usuarioService.VerficarDisponibilidadeEmail(registroUsuarioDTO.Email);
            _usuarioService.VerficarIdadeUsuario(registroUsuarioDTO.DataNascimento);
            await _documentoService.VerificarDisponibilidadeDocumento(registroUsuarioDTO.Documento.Numero);
        }

        private Documento PegarDocumento(DocumentoDTO documentoDto)
        {
            return _mapper.Map<Documento>(documentoDto);                        
        }
    }
}
