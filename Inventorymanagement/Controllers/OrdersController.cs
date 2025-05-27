using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace Inventorymanagement.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InventoryContext _context;

        public OrdersController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var inventoryContext = _context.Orders.Include(o => o.Item).Include(o => o.Supplier);
            return View(await inventoryContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Item)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,ItemID,SupplierID,Quantity,OrderDate,IsDelivered")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Fetch the item from the database to update its stock quantity
                var item = await _context.Items.FindAsync(order.ItemID);

                if (item == null)
                {
                    return NotFound();  // If item doesn't exist, return error
                }

                if (item.Quantity < order.Quantity)
                {
                    ModelState.AddModelError("", "Insufficient stock for this order.");
                    return View(order);  // If not enough stock, show error message
                }

                // Decrease the stock quantity of the item
                item.Quantity -= order.Quantity;

                // Log the stock movement for the ordered item
                LogStockMovement(item, "Item Ordered", order.Quantity);

                // Create or update the low stock alert in the Alert model
                UpdateLowStockAlert(item);

                // Add the order to the database
                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", order.ItemID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", order.SupplierID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", order.ItemID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", order.SupplierID);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,ItemID,SupplierID,Quantity,OrderDate,IsDelivered")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing order to compare quantities
                    var existingOrder = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderID == order.OrderID);
                    if (existingOrder == null)
                    {
                        return NotFound();  // If the order doesn't exist, return error
                    }

                    // Fetch the item being ordered
                    var item = await _context.Items.FindAsync(order.ItemID);
                    if (item == null)
                    {
                        return NotFound();  // If the item doesn't exist, return error
                    }

                    // Determine if stock has increased or decreased
                    int quantityDifference = order.Quantity - existingOrder.Quantity;
                    if (quantityDifference != 0)
                    {
                        // Update the stock quantity based on the order difference (increase or decrease)
                        item.Quantity -= quantityDifference;

                        // Log the stock movement (decreasing/increasing stock)
                        LogStockMovement(item, "Item Order Edited", quantityDifference);

                        // Create or update the low stock alert after the edit
                        UpdateLowStockAlert(item);
                    }

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "Name", order.ItemID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", order.SupplierID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Item)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        // Helper function to log stock movement (add a stock movement record)
        private void LogStockMovement(Item item, string action, int orderQuantity)
        {
            var userId = GetCurrentUserSupplierID();
            if (userId == -1)
            {
                Console.WriteLine("Error: Invalid UserID, cannot log stock movement.");
                return;
            }

            var stockMovement = new StockMovement
            {
                ItemID = item.ItemID,
                UserID = userId,
                OldQuantity = item.Quantity + orderQuantity, // The stock before the order
                NewQuantity = item.Quantity, // The stock after the order
                Action = action,
                MovementDate = DateTime.Now
            };

            _context.StockMovements.Add(stockMovement);
            _context.SaveChanges();
        }

        // Helper function to get the current user's SupplierID
        private int GetCurrentUserSupplierID()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.UserID.ToString() == userId);
            return user?.Role == UserRole.Supplier ? user.UserID : -1;
        }

        // Helper function to update low stock alert in the Alert model
        private void UpdateLowStockAlert(Item item)
        {
            if (item.Quantity <= item.LowStockThreshold)
            {
                // Check if there's an existing low stock alert
                var existingAlert = _context.Alerts.FirstOrDefault(a => a.ItemID == item.ItemID && a.IsActive);

                if (existingAlert == null)
                {
                    // Create a new alert if there isn't one already
                    var alert = new Alert
                    {
                        ItemID = item.ItemID,
                        IsActive = true,
                        Message = $"Stock is low for item {item.Name}. Please restock.",
                        CreatedDate = DateTime.Now
                    };
                    _context.Alerts.Add(alert);
                }
                else
                {
                    // Update the existing alert to ensure it's active
                    existingAlert.IsActive = true;
                    existingAlert.Message = $"Stock is low for item {item.Name}. Please restock.";
                    existingAlert.CreatedDate = DateTime.Now;
                    _context.Alerts.Update(existingAlert);
                }

                _context.SaveChanges();
            }
        }
    }
}
