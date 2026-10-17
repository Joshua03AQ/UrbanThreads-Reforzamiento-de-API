using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using UbranThreads_API.Data;
using UbranThreads_API.Repositories.IRepositories;

namespace UbranThreads_API.Repositories
{
    public class UsuarioRepositorycs : IUsuarioRepositoy
    {

        private readonly UrbanThreadsContext? _context;
        private readonly IPasswordHasher<Usuario>? _passwordHasher;

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _context.Usuarios.FindAsync(email);
        }

        public async Task<Usuario> GetUserByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GetUserByUsernameAsync(string username)
        {
            return await _context.Usuarios.SingleOrDefaultAsync(u => u.username == username);
        }

        public async Task RegisterUserAsync(Usuario usuario, string password)
        {
            usuario.passwordHash = _passwordHasher.HashPassword(usuario, password);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var usuario = await GetUserByUsernameAsync(username);
            if (usuario == null) return false;

            var verificarResultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.passwordHash, password);
            return verificarResultado == PasswordVerificationResult.Success;
        }

        public async Task<bool> ValidateEmailAsync(string email, string password)
        {
            var correo = await GetUserByEmailAsync(email);
            if (correo == null) return false;

            var verificarResultado = _passwordHasher.VerifyHashedPassword(correo, correo.passwordHash, password);
            return verificarResultado == PasswordVerificationResult.Success;
        }
    }
}
