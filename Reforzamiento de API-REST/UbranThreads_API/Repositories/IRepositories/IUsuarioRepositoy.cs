using SharedModels;

namespace UbranThreads_API.Repositories.IRepositories
{
    public interface IUsuarioRepositoy
    {
        Task<Usuario> GetUserByUsernameAsync(string username);
        Task<Usuario> GetUserByEmailAsync(string email);
        Task<Usuario> GetUserByIdAsync(int id);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<bool> ValidateEmailAsync(string email, string password);
        Task RegisterUserAsync(Usuario usuario, string password);
    }
}
