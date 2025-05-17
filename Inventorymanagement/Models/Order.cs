using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int ItemID { get; set; }
        public virtual Item? Item { get; set; }

        [Required]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public bool IsDelivered { get; set; } = false;
    }
}
