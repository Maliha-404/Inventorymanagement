using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class StockMovement
    {
        [Key]
        public int MovementID { get; set; }

        [Required]
        public int ItemID { get; set; } 

        [Required]
        public int UserID { get; set; } 

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int OldQuantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int NewQuantity { get; set; }

        [Required]
        [Display(Name = "Date/Time")]
        public DateTime MovementDate { get; set; } = DateTime.Now;
        [Required]
        public string? Action { get; set; }
        [ForeignKey("ItemID")]
        public virtual Item? Item { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}