using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.Data;

namespace Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class SliderImagesController : Controller
    {
        private readonly AppDbContext _context;

        public SliderImagesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Dashboard/SliderImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.SliderImageS.ToListAsync());
        }

        // GET: Dashboard/SliderImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliderImage = await _context.SliderImageS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sliderImage == null)
            {
                return NotFound();
            }

            return View(sliderImage);
        }

        // GET: Dashboard/SliderImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/SliderImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImage sliderImage, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ModelState.AddModelError(nameof(sliderImage.Image), "Image is required.");
                    return View(sliderImage);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);

                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/sliderImages");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var savePath = Path.Combine(imageDirectory, imageName);

                await using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                sliderImage.Image = $"/img/sliderImages/{imageName}";

                _context.Add(sliderImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sliderImage);
        }

        // GET: Dashboard/SliderImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliderImage = await _context.SliderImageS.FindAsync(id);
            if (sliderImage == null)
            {
                return NotFound();
            }
            return View(sliderImage);
        }

        // POST: Dashboard/SliderImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Title,SortOrder")] SliderImage sliderImage)
        {
            if (id != sliderImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sliderImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderImageExists(sliderImage.Id))
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
            return View(sliderImage);
        }

        // GET: Dashboard/SliderImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliderImage = await _context.SliderImageS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sliderImage == null)
            {
                return NotFound();
            }

            return View(sliderImage);
        }

        // POST: Dashboard/SliderImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sliderImage = await _context.SliderImageS.FindAsync(id);
            if (sliderImage != null)
            {
                _context.SliderImageS.Remove(sliderImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderImageExists(int id)
        {
            return _context.SliderImageS.Any(e => e.Id == id);
        }
    }
}
