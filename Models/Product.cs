using System.ComponentModel.DataAnnotations;

namespace Cutbitfarm.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama produk wajib diisi")]
        [StringLength(150, ErrorMessage = "Nama maksimal 150 karakter")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Deskripsi maksimal 500 karakter")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Harga wajib diisi")]
        [Range(1000, 100000000, ErrorMessage = "Harga tidak masuk akal")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}


// using System.ComponentModel.DataAnnotations;

// namespace Cutbitfarm.Models
// {
//     public class Product
//     {
//         public int Id { get; set; }

//         [Required]
//         [MaxLength(150)]
//         public string Name { get; set; }

//         public string? Description { get; set; }

//         [Required]
//         public decimal Price { get; set; }

//         public string? ImageUrl { get; set; }

//         // Optional (untuk masa depan)
//         public DateTime CreatedAt { get; set; } = DateTime.Now;
//     }
// }


// namespace Cutbitfarm.Models
// {
//     public class Product
//     {
//         public int Id { get; set; }
//         public string? Name { get; set; }
//         public string? Description { get; set; }
//         public decimal Price { get; set; }
//         public string? ImageUrl { get; set; }
//     }
// }