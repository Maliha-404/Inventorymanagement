using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InventoryManagement.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        [Phone]
        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}