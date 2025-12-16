using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.models
{
    public class ProductModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Stock { get; set; } = string.Empty;
        public string Images { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
