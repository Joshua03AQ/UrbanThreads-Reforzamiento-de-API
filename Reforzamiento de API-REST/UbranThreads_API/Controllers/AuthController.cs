using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UbranThreads_API.Repositories.IRepositories;
using AutoMapper;
using UbranThreads_API.Dto;
using SharedModels;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UbranThreads_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository? _usuarioRepo;
        private readonly IMapper? _mapper;
        private readonly IConfiguration? _config;

        public AuthController(IUsuarioRepository usuarioRepo, IMapper mapper, IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUsuarioDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existenciaUsuario = await _usuarioRepo.GetUserByUsernameAsync(model.Username);
            if (existenciaUsuario != null)
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            var user = _mapper.Map<Usuario>(model);    

            try
            {
                await _usuarioRepo.RegisterUserAsync(user, model.Password);
                return Ok("Usuario registrado exitosamente.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioRepo.GetUserByUsernameAsync(model.Username);

            if (User == null || !await _usuarioRepo.ValidateUserAsync(model.Username, model.Password))
            {
                return Unauthorized("Credenciales invaildas");
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var jwtSetting = _config.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSetting.GetValue<string>("Key"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat , DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub , usuario.username),
                    new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.username)

                }),
                Issuer = jwtSetting.GetValue<string>("Issuer"),
                Audience = jwtSetting.GetValue<string>("Audience"),
                Expires = DateTime.UtcNow.AddHours(1), // Tiempo de expiracion del Token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
