using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SistemaEstoque.API.Models;
using SistemaEstoque.API.Validations;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

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
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<Usuario> signInManager,
                               UserManager<Usuario> userManager,
                               IMapper mapper,
                               INotificador notificador,
                               IDocumentoService documentoService,
                               IUsuarioService usuarioService,
                               IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _notificador = notificador;
            _documentoService = documentoService;
            _usuarioService = usuarioService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("cadastrar-usuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CadastrarUsuario(RegistroUsuarioDTO registroUsuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            await ValidarCadastro(registroUsuarioDTO);

            try
            {
                CapitalizarNome(registroUsuarioDTO);

                var usuario = _mapper.Map<Usuario>(registroUsuarioDTO);                    

                if (_notificador.TemNotificacao())
                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));

                var result = await _userManager.CreateAsync(usuario, registroUsuarioDTO.Senha);

                if (!result.Succeeded)
                {
                    var documento = PegarDocumento(registroUsuarioDTO.Documento);
                    await _documentoService.ExcluirDocumento(documento);

                    foreach (var item in result.Errors)
                    {
                        var notificacao = new Notificacao(item.Description);
                        _notificador.AdicionarNotificacao(notificacao);
                    }

                    return BadRequest(new ErrorModel(_notificador.ObterNotificacoes()));
                }

                await _signInManager.SignInAsync(usuario, false);

                return Ok(new JwtToken(await GerarJwt(usuario.Email), 
                                                      DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras)));
            }
            catch (EntidadeExcepetions ex)
            {
                return BadRequest($"{ex.InnerException?.Message} {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("entrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, true);
           
            if (!result.Succeeded)
                return BadRequest("Login não realizado");

            var usuario = await _userManager.FindByEmailAsync(login.Email);

            if (!usuario.Ativo)
                 return Unauthorized("Usuário inativo");

            return Ok(new JwtToken(await GerarJwt(login.Email), 
                                                   DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras)));
        }

        private async Task ValidarCadastro(RegistroUsuarioDTO registroUsuarioDTO)
        {
            await _usuarioService.VerficarDisponibilidadeEmail(registroUsuarioDTO.Email);
            _usuarioService.VerficarIdadeUsuario(registroUsuarioDTO.DataNascimento);
            await _documentoService.VerificarDisponibilidadeDocumento(registroUsuarioDTO.Documento.NumeroDocumento);
        }

        private Documento PegarDocumento(DocumentoDTO documentoDto)
        {
            return _mapper.Map<Documento>(documentoDto);
        }

        private string CapitalizarNome(RegistroUsuarioDTO registroUsuarioDTO) 
        {
            registroUsuarioDTO.Nome = CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(registroUsuarioDTO.Nome);
            return registroUsuarioDTO.Nome;
        }

        private async Task<string> GerarJwt(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            var identityClaims = new ClaimsIdentity(new[]
            {
                new Claim(type: ClaimTypes.NameIdentifier, value: usuario.Id.ToString())
            });

            var claims = await _userManager.GetClaimsAsync(usuario);
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
