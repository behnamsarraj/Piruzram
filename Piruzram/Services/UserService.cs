using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Models;

namespace Piruzram.Services;

public interface IUserService
{
    Task<ApplicationUser?> GetUserByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationUsers
            .Where(w => w.Email == userId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}