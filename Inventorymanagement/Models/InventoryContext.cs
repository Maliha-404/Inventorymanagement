using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;

namespace InventoryManagement.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }
        public InventoryContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;
                      Database=InventoryDB;
                      Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal precision for Price
            modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            // Relationships
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Supplier)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SupplierID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockMovement>()
                .HasOne(m => m.Item)
                .WithMany(i => i.StockMovements)
                .HasForeignKey(m => m.ItemID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.Item)
                .WithMany(i => i.Alerts)
                .HasForeignKey(a => a.ItemID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Item)
                .WithMany()
                .HasForeignKey(o => o.ItemID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany()
                .HasForeignKey(o => o.SupplierID);
        }
    }
}