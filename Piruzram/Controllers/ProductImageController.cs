using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Models;
using Piruzram.ViewModel;

namespace Piruzram.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductImage
        public async Task<IActionResult> Index()
        {
            List<ProductImageViewModel> productImageViewModel = new List<ProductImageViewModel>();
            var ProductImages = await _context.ProductImages.Include(p => p.Product).ToListAsync();
            foreach (var productImage in ProductImages)
            {
                ProductImageViewModel viewModel = new ProductImageViewModel()
                {
                    Id = productImage.Id,
                    ImageAddress = productImage.ImageAddress
                };
                productImageViewModel.Add(viewModel);
            }
            return View(productImageViewModel);
        }

        // GET: ProductImage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productImages = await _context.ProductImages.
                FirstOrDefaultAsync(n => n.Id == id);
            if (productImages == null)
            {
                return NotFound();
            }
            ProductImageViewModel productImageViewModel = new ProductImageViewModel()
            {
                Id = productImages.Id,
                ImageAddress = productImages.ImageAddress    
            };

            return View(productImageViewModel);
        }

        // GET: ProductImage/Create
        public async Task<IActionResult> Create()
        {
            ProductImageViewModel productImageViewModel = new ProductImageViewModel();
            return View("Form", productImageViewModel);
        }

        // GET: ProductImage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }
            List<SelectListItem> productDropDown = _context.Products.Select(product => new SelectListItem
            {
                Value = product.Id.ToString(),
                Text = product.Name
            }).ToList<SelectListItem>();

            ProductImageViewModel productModel = new ProductImageViewModel()
            {
                Id = productImage.Id,
                ImageAddress = productImage.ImageAddress
            };

            return View("Form", productModel);
        }

        // GET: ProductImage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Products.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("Save")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(int id, ProductImageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }

            #region Add Mode
            if (viewModel.Id == 0)
            {
                ProductImage productImage = new ProductImage()
                {
                    ImageAddress = viewModel.ImageAddress,
                    ProductId = viewModel.ProductId,
                    Product = _context.Products.FirstOrDefault(n=> n.Id == viewModel.ProductId),
                };
                _context.ProductImages.Add(productImage);

            }
            #endregion
            #region Edit Mode
            else
            {
                ProductImage productImage = await _context.ProductImages.FindAsync(viewModel.Id);
                productImage.ImageAddress = viewModel.ImageAddress;
                productImage.ProductId = viewModel.ProductId;
                productImage.Product = _context.Products.FirstOrDefault(n => n.Id == viewModel.ProductId);
                _context.Update(productImage);
            }

            #endregion

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
