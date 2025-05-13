using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }

        [Required]
        public int ItemID { get; set; }
        public virtual Item Item { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(255)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
