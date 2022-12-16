using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Enums;
using Piruzram.Models;

namespace Piruzram.Services;

public interface ICartService
{
    Task<Cart> CreateCartAsync(string userId, CancellationToken cancellationToken = default);
}

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public CartService(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<Cart> CreateCartAsync(string userId, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userService.GetUserByUserIdAsync(userId);
        if (applicationUser is null)
            throw new UnauthorizedAccessException();
        
        var carts = _context.Carts.Where(n => n.ApplicationUser!.Email == userId);
        var cart = await carts.FirstOrDefaultAsync(w => w.Status == CartStatus.Active, cancellationToken: cancellationToken);
        if (cart is not null)
            return cart;

        var newCart = new Cart
        {
            ApplicationUserId = applicationUser!.Id,
            Status = CartStatus.Active
        };
        _context.Carts.Add(newCart);
        await _context.SaveChangesAsync(cancellationToken);
        return newCart;
    }
}