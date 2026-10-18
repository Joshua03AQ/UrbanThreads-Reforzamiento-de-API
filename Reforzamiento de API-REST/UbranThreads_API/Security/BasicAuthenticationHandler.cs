using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using UbranThreads_API.Repositories.IRepositories;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text;

namespace UbranThreads_API.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUsuarioRepository _usuarioRepositoy;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUsuarioRepository usuarioRepositoy) : base(options, logger, encoder, clock)
        {
            _usuarioRepositoy = usuarioRepositoy;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header or Email/Password Headers");
            }

            try
            {
                var authHeader = Request.Headers["Authorization"];
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter)).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                var usuario = await _usuarioRepositoy.ValidateUserAsync(username, password);

                if (!usuario)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }

                var claims = new[] 
                { 
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username)
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);

            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
