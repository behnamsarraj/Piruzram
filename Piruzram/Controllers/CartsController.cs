using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Models;

namespace Piruzram.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public System.Security.Claims.ClaimsPrincipal LocalUser { get; set; }

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Carts
                .Include(c => c.ApplicationUser)
                .Where(n => n.ApplicationUser.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }



        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> Create()
        {
            Cart cart = new Cart();
            if (User != null)
            {
                LocalUser = User;
            }
            IQueryable<ApplicationUser> ApplicationUsers = _context.ApplicationUsers.Where(n => n.Email == LocalUser.Identity.Name);
            cart.ApplicationUser = ApplicationUsers.FirstOrDefault();
            if (cart.ApplicationUser == null)
            {
                return NotFound();
            }

            IQueryable<Cart> Carts = _context.Carts.Where(n => n.ApplicationUser.Email == LocalUser.Identity.Name);
            foreach (Cart cartItem in Carts)
            {
                if (cartItem.Status == Enums.CartStatus.Active)
                {
                    return RedirectToAction(nameof(Index));
                }
            }


            cart.ApplicationUserId = cart.ApplicationUser.Id;
            cart.Status = Enums.CartStatus.Active;
            _context.Add(cart);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Carts/Paid/5
        public async Task<IActionResult> Paid(int? id)
        {

            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }
            Cart cart = _context.Carts.FirstOrDefault(n => n.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.Status = Enums.CartStatus.Paid;
            try
            {
                _context.Update(cart);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Carts/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
        {

            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }
            Cart cart = _context.Carts.FirstOrDefault(n => n.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.Status = Enums.CartStatus.Canceled;
            try
            {
                _context.Update(cart);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(cart.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cart.ApplicationUserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cart.ApplicationUserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
