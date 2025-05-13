using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(x => x.Category)
                .ToListAsync());
        }

        // GET: Dashboard/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category) // أهم تعديل: تحميل علاقة الـ Category
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Dashboard/Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ModelState.AddModelError(nameof(product.Image), "Image is required.");

                    // Get categories to repopulate dropdown
                    var categories = await _context.Categories.ToListAsync();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

                    return View(product);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);

                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var savePath = Path.Combine(imageDirectory, imageName);

                await using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                product.Image = $"/img/Products/{imageName}";

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Get categories to repopulate dropdown when ModelState is not valid
            var allCategories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(allCategories, "Id", "Name", product.CategoryId);

            return View(product);
        }


        // GET: Dashboard/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.ToArrayAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile Image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldProduct = await _context.Products.FindAsync(id);
                    if (oldProduct == null)
                    {
                        return NotFound();
                    }

                    // تحديث الصورة إذا تم تحميل واحدة جديدة
                    if (Image != null)
                    {
                        var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var filePath = Path.Combine(uploadPath, imageName);
                        await using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        oldProduct.Image = $"/img/Products/{imageName}";
                    }

                    // تحديث الحقول الأساسية بما فيها CategoryId
                    oldProduct.Name = product.Name;
                    oldProduct.Price = product.Price;
                    oldProduct.Description = product.Description;
                    oldProduct.CategoryId = product.CategoryId; // التأكد من تحديث هذا الحقل

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError("", "تعذر الحفظ بسبب تعديل البيانات من قبل مستخدم آخر. يرجى المحاولة مرة أخرى.");
                    return View(product);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "حدث خطأ غير متوقع: " + ex.Message);
                    return View(product);
                }
            }

            // إذا كان ModelState غير صالح، إعادة تعيين ViewBag.Categories
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(product);
        }
        // GET: Dashboard/Products/Delete/5
        // GET: Dashboard/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)  // أهم تعديل: تحميل علاقة الـ Category
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Dashboard/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
