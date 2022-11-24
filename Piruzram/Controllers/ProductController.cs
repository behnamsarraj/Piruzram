using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Piruzram.Data;
using Piruzram.Models;
using Piruzram.ViewModel;

namespace Piruzram.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductViewModel> productViewModel = new List<ProductViewModel>();
            var products = await _context.Products.Include(p => p.ProductCategory).ToListAsync();
            foreach (var product in products)
            {
                ProductViewModel viewModel = new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.ProductCategory.Name,
                    ProductImages = _context.ProductImages.Where(n => n.ProductId == product.Id).ToList<ProductImage>(),
                };
                productViewModel.Add(viewModel);
            }
            return View(productViewModel);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.
                FirstOrDefaultAsync(n => n.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = _context.ProductCategories.FirstOrDefault(n => n.Id == product.ProductCateguryId).Name,
                ProductImages = _context.ProductImages.Where(n => n.ProductId == id).ToList<ProductImage>()

            };

            return View(productViewModel);
        }
        public async Task<IActionResult> Create()
        {
            var categoriesDropDown = await _context.ProductCategories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToListAsync();
            return View("Form", new ProductViewModel { CategoriesDropDown = categoriesDropDown });
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            List<SelectListItem> categoriesDropDown = _context.ProductCategories.Select(categury => new SelectListItem
            {
                Value = categury.Id.ToString(),
                Text = categury.Name
            }).ToList<SelectListItem>();

            ProductViewModel productModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name,
                CategoriesDropDown = categoriesDropDown,
                ProductImages = _context.ProductImages.Where(n => n.ProductId == id).ToList<ProductImage>(),

            };

            return View("Form", productModel);
        }
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
        public async Task<IActionResult> Save(int id, ProductViewModel viewModel)
        {
            /*
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }
            */
            #region Add Mode
            if (viewModel.Id == 0)
            {
                Product product = new Product()
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                    Description = viewModel.Description,
                    ProductCateguryId = viewModel.CategoryId,
                    ProductCategory = _context.ProductCategories.FirstOrDefault(n => n.Id == viewModel.CategoryId),
                    ApplicationUserId = Convert.ToString(_context.ApplicationUsers.FirstOrDefault(n => n.Email == User.Identity.Name).Id),
                    ApplicationUser = _context.ApplicationUsers.FirstOrDefault(n => n.Email == User.Identity.Name)

                };
                _context.Products.Add(product);

            }
            #endregion
            #region Edit Mode
            else
            {
                Product product = await _context.Products.FindAsync(viewModel.Id);
                product.Images = await _context.ProductImages.Where(n => n.ProductId == viewModel.Id).ToArrayAsync();
                product.Name = viewModel.Name;
                product.Description = viewModel.Description;
                product.Price = viewModel.Price;
                product.ProductCateguryId = viewModel.CategoryId;
                product.ProductCategory = _context.ProductCategories.FirstOrDefault(n => n.Id == viewModel.CategoryId);
                product.Images = viewModel.ProductImages;
                _context.Update(product);
                

                
            }

            #endregion

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductImages([Bind("ProductImages")]ProductViewModel product)
        {
            product.ProductImages.Add(new ProductImage());
            return PartialView("ProductImages", product);
        }
    }
}
