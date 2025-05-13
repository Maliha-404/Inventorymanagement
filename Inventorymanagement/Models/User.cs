using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
   
    public enum UserRole
    {
        Manager,
        WarehouseStaff,
        Supplier
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; } 
    }
}
