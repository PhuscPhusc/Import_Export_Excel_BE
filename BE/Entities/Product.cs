using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        [Column("Price")]
        public string Price { get; set; } = string.Empty;

        [StringLength(200)]
        [Column("Stock")]
        public string Stock { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Images")]
        public string Images { get; set; } = string.Empty;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    }
}
