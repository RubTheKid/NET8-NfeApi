using Nfe.Auth.Models;

namespace Nfe.Auth;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<bool> EmailExists(string email);
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task<int> SaveChangesAsync();
}