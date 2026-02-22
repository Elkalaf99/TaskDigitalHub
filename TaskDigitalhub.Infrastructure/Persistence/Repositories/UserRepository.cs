using Microsoft.EntityFrameworkCore;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Entities;
using TaskDigitalhub.Infrastructure.Persistence;

namespace TaskDigitalhub.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ProjectManagementDbContext _context;

    public UserRepository(ProjectManagementDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
