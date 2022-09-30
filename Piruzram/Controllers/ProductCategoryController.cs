using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Models;
using Piruzram.ViewModel;

namespace Piruzram.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.ProductCategories.ToListAsync();

            var viewModel = categories
                .Select(category => new ProductCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToList();
            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new ProductCategoryViewModel();
            return View("Form", viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new ProductCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductCategoryViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("From", viewModel);
            //}

            if (viewModel.Id == 0)
            {
                var category = new ProductCategory
                {
                    Name = viewModel.Name
                };
                _context.Add(category);
            }
            else
            {
                var category = await _context.ProductCategories.FindAsync(viewModel.Id);
                category.Name = viewModel.Name;
                _context.Update(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
