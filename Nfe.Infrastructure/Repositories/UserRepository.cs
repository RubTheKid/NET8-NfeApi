using Microsoft.EntityFrameworkCore;
using Nfe.Auth;
using Nfe.Auth.Models;
using Nfe.Infrastructure.Data;

namespace Nfe.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NfeDbContext _context;

    public UserRepository(NfeDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> Create(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task<User> Update(User user)
    {
        _context.Users.Update(user);
        return user;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}