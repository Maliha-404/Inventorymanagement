using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 1000, ErrorMessage = "Threshold must be 1-1000")]
        public int LowStockThreshold { get; set; } = 10;
        
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public ICollection<StockMovement>? StockMovements { get; set; }
        public ICollection<Alert>? Alerts { get; set; }
    }
}
