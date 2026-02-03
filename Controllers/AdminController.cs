using Cutbitfarm.Data;
using Cutbitfarm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Cutbitfarm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // ================= CREATE =================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("", "Gambar produk wajib diisi");
                return View(product);
            }

            // validsi Tipe File
            var allowedExtensions = new[] {".jpg", ".jpeg", ".png"};
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Format gambar tidak valid. Hanya .jpg, .jpeg, .png yang diperbolehkan.");
                return View(product);
            }
            
            // validsi Ukuran File (maks 2MB)
            if (imageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("", "Ukuran gambar maksimal 2MB");
                return View(product);
            }

            // folder upload
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads/products");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // 🧾 Nama file unik
            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // 💾 Simpan path ke DB
            product.ImageUrl = "/uploads/products/" + fileName;
            product.CreatedAt = DateTime.Now;

            _context.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // public IActionResult Create(Product product)
        // {
        //     if (!ModelState.IsValid)
        //         return View(product);

        //     product.CreatedAt = DateTime.Now;

        //     _context.Products.Add(product);
        //     _context.SaveChanges();

        //     return RedirectToAction(nameof(Index));
        // }


        // GET: Admin/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Admin/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Edit(Product product)
        // {
        //     if (!ModelState.IsValid)
        //         return View(product);

        //     _context.Products.Update(product);
        //     _context.SaveChanges();

        //     TempData["Success"] = "Produk berhasil diperbarui";
        //     return RedirectToAction(nameof(Index));
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
        {
            var productDb = await _context.Products.FindAsync(product.Id);
            if (productDb == null)
                return NotFound();

            // Validasi manual field penting
            if (string.IsNullOrWhiteSpace(product.Name))
                ModelState.AddModelError("Name", "Nama wajib diisi");

            if (product.Price <= 0)
                ModelState.AddModelError("Price", "Harga tidak valid");

            if (!ModelState.IsValid)
                return View(product);

            productDb.Name = product.Name;
            productDb.Description = product.Description;
            productDb.Price = product.Price;

            // ===== JIKA ADA GAMBAR BARU =====
            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLower();
                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "Format gambar tidak valid");
                    return View(product);
                }

                // Hapus gambar lama
                if (!string.IsNullOrEmpty(productDb.ImageUrl))
                {
                    var oldPath = Path.Combine(
                        _environment.WebRootPath,
                        productDb.ImageUrl.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var folder = Path.Combine(_environment.WebRootPath, "uploads/products");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + ext;
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                productDb.ImageUrl = "/uploads/products/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        // {
        //     if (!ModelState.IsValid)
        //         return View(product);

        //     var productDb = await _context.Products.FindAsync(product.Id);
        //     if (productDb == null)
        //         return NotFound();

        //     // Update field biasa
        //     productDb.Name = product.Name;
        //     productDb.Description = product.Description;
        //     productDb.Price = product.Price;

        //     // ===== JIKA ADA GAMBAR BARU =====
        //     if (imageFile != null && imageFile.Length > 0)
        //     {
        //         // 1. Hapus file lama
        //         if (!string.IsNullOrEmpty(productDb.ImageUrl))
        //         {
        //             var oldPath = Path.Combine(
        //                 Directory.GetCurrentDirectory(),
        //                 "wwwroot",
        //                 productDb.ImageUrl.TrimStart('/')
        //             );

        //             if (System.IO.File.Exists(oldPath))
        //                 System.IO.File.Delete(oldPath);
        //         }

        //         // 2. Simpan file baru
        //         var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        //         Directory.CreateDirectory(uploadsFolder);

        //         var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        //         var filePath = Path.Combine(uploadsFolder, fileName);

        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await imageFile.CopyToAsync(stream);
        //         }

        //         productDb.ImageUrl = "/images/" + fileName;
        //     }

        //     _context.Update(productDb);
        //     await _context.SaveChangesAsync();

        //     TempData["Success"] = "Produk berhasil diperbarui";
        //     return RedirectToAction(nameof(Index));
        // }


        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Admin/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // ===== HAPUS FILE GAMBAR =====
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    product.ImageUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Produk berhasil dihapus";
            return RedirectToAction(nameof(Index));
        }


    }
}
