using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers
{
    [Authorize]  // Ensure only authenticated users can access the dashboard
    public class DashboardController : Controller
    {
        private readonly InventoryContext _context;

        public DashboardController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Index
        public async Task<IActionResult> Index()
        {
            // Data fetching for Dashboard
            var userCount = await _context.Users.CountAsync();
            var itemCount = await _context.Items.CountAsync();
            var lowStockItems = await _context.Items.Where(i => i.Quantity <= i.LowStockThreshold).ToListAsync();
            var orderCount = await _context.Orders.CountAsync();

            // Fetching data for Suppliers (total count, for example)
            var supplierCount = await _context.Suppliers.CountAsync();

            // Pass the data to the View
            ViewData["UserCount"] = userCount;
            ViewData["ItemCount"] = itemCount;
            ViewData["LowStockItems"] = lowStockItems;
            ViewData["LowStockCount"] = lowStockItems.Count;
            ViewData["OrderCount"] = orderCount;
            ViewData["SupplierCount"] = supplierCount; // Pass supplier data

            return View();
        }
    }
}
