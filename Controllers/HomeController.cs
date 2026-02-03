using Cutbitfarm.Data;
using Cutbitfarm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cutbitfarm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Edukatif()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Karakteristik()
        {

            return View();
        }

        public IActionResult Perawatan()
        {
            return View();
        }

        public IActionResult Video()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            // Kita bikin List (Array) produk pura-pura di sini
            // Nanti kalau sudah connect database, ini diganti query database
            // var products = new List<Product>
            // {
            //     new Product
            //     {
            //         Id = 1,
            //         Name = "Ayam Ekor Lidi Jantan",
            //         Description = "Ayam ekor lidi jantan sehat, usia 8 bulan, siap untuk dikembangbiakkan.",
            //         Price = 500000,
            //         ImageUrl = "/images/ayamekorlidi1.jpg"
            //     },
            //     new Product
            //     {
            //         Id = 2,
            //         Name = "Ayam Ekor Lidi Betina",
            //         Description = "Ayam ekor lidi betina produktif, usia 6 bulan, cocok untuk peternakan.",
            //         Price = 400000,
            //         ImageUrl = "/images/ayamekorlidi2.jpg"
            //     },
            //     new Product
            //     {
            //         Id = 3,
            //         Name = "Pakan Ayam Organik",
            //         Description = "Pakan alami berkualitas tinggi untuk menjaga kesehatan ayam.",
            //         Price = 150000,
            //         ImageUrl = "/images/ayamekorlidi3.jpg"
            //     }
            // };

            // "products" ini kita lempar ke View (mirip return view('products', compact('products')))
            return View(products);
        }

        public IActionResult Order(int? id) // Perhatikan: ada parameter 'id'
        {
            if (id == null)
            {
                return RedirectToAction("Products"); // Kalau ga ada ID, tendang balik ke menu produk
            }


            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
            // --- Simulasi Database (Ceritanya kita ambil data lagi) ---
            // Di aplikasi nyata, bagian ini tidak perlu ditulis ulang, tapi pakai Database
            // var products = new List<Product>
            // {
            //     new Product { Id = 1, Name = "Ayam Ekor Lidi Jantan", Price = 500000, ImageUrl = "/images/ayamekorlidi1.jpg" },
            //     new Product { Id = 2, Name = "Ayam Ekor Lidi Betina", Price = 400000, ImageUrl = "/images/ayamekorlidi2.jpg" },
            //     new Product { Id = 3, Name = "Pakan Ayam Organik", Price = 150000, ImageUrl = "/images/ayamekorlidi3.jpg" }
            // };

            // Cari produk yang ID-nya cocok (Laravel: Product::find($id))
            // var productToOrder = products.FirstOrDefault(p => p.Id == id);

            // Kalau produk tidak ditemukan (misal user iseng ngetik ID 999)
            // if (productToOrder == null)
            // {
            //     return NotFound();
            // }

            // Lempar SATU data produk ke View
            // return View(productToOrder);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
