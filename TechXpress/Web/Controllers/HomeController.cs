using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModels;
using Infrastructure.Data;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;


        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult AboutUs() {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .ToList();

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems.Select(c => new CartItemViewModel
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    Qty = c.Qty,
                    Image = c.Product.Image
                }).ToList(),
                Total = cartItems.Sum(c => c.Qty * c.Product.Price)
            };

            return View(viewModel);
        }

        public IActionResult GetCartItems()
        {
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .ToList();

            var viewModel = cartItems.Select(c => new CartItemViewModel
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                Price = c.Product.Price,
                Qty = c.Qty,
                Image = c.Product.Image
            }).ToList();

            return PartialView("_CartItemsPartial", viewModel); 
        }

        public IActionResult GetCartSummary()
        {
            var cartItems = _context.Carts.Include(c => c.Product).ToList();

            var count = cartItems.Sum(c => c.Qty);
            var total = cartItems.Sum(c => c.Qty * c.Product.Price);

            return Json(new { count, total });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.ProductId == request.ProductId);

            if (cartItem != null)
            {
                cartItem.Qty++;
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    ProductId = request.ProductId,
                    Qty = 1
                });
            }

            _context.SaveChanges();
            return Ok();
        }

        public class AddToCartRequest
        {
            public int ProductId { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FilterByCategory([FromBody] List<int> selectedCategoryIds)
        {
            var filteredProducts = _context.Products
                .Include(p => p.Category)
                .Where(p => selectedCategoryIds.Contains(p.CategoryId ?? 0))
                .ToList();

            return PartialView("_ProductCardsPartial", filteredProducts);
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Products = await _context.Products
                    .Include(x => x.Category)
                    .ToListAsync()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Product()
        {
            var viewModel = new ProductViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Products = await _context.Products
                    .Include(x => x.Category)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
