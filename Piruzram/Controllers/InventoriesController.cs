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
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Inventories.Include(i => i.Cart).Include(i => i.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Cart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            //ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Count")] Inventory inventory)
        {
            inventory.Cart = _context.Carts.Where(n => n.ApplicationUser.Email == User.Identity.Name).FirstOrDefault(n => n.Status == Enums.CartStatus.Active);
            if (inventory.Cart == null)
            {
                CartsController cartsController = new CartsController(_context);
                cartsController.LocalUser = User;
                cartsController.Create();
                IQueryable<Cart> carts = _context.Carts.Where(n => n.ApplicationUser.Email == User.Identity.Name);
                inventory.Cart = carts.FirstOrDefault(n => n.Status == Enums.CartStatus.Active);

            }
            if (inventory.Cart != null && inventory.ProductId != null)
            {
                inventory.CartId = _context.Carts.FirstOrDefault(n => n == inventory.Cart).Id;
                inventory.Product = _context.Products.FirstOrDefault(n => n.Id == inventory.ProductId);
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id", inventory.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id", inventory.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Count,CartId")] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Id))
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
            ViewData["CartId"] = new SelectList(_context.Carts, "Id", "Id", inventory.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.Cart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventories'  is null.");
            }
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
          return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
