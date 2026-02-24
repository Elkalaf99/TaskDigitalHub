using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string userName);
    Task<User> AddAsync(User user);
    void Update(User user);
}
