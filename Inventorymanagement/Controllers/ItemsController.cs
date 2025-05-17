using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inventorymanagement.Controllers
{
    [Authorize] // Ensures only authenticated users can access these actions
    public class ItemsController : Controller
    {
        private readonly InventoryContext _context;

        public ItemsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var items = _context.Items.Include(i => i.Supplier);
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userRole == "WarehouseStaff")
            {
                // Warehouse Staff can only view items, no editing or deleting
                return View(await items.ToListAsync());
            }
            else if (userRole == "Supplier")
            {
                // Suppliers can only update stock quantity, not see everything
                var supplierItems = await items.Where(i => i.SupplierID == GetCurrentUserSupplierID()).ToListAsync();
                return View(supplierItems);
            }

            // Managers can view all items
            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("ItemID,Name,Quantity,Price,LowStockThreshold,SupplierID")] Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Adding the item to the database
                    _context.Add(item);
                    await _context.SaveChangesAsync();

                    // Log stock movement after creating item
                    LogStockMovement(item, "Item Created");

                    // Create alert for low stock
                    CreateLowStockAlert(item);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception or output to view for debugging
                    ModelState.AddModelError("", $"An error occurred while saving the item: {ex.Message}");
                    return View(item);
                }
            }

            // If model is not valid, repopulate the Supplier select list and return the view with the error
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,Name,Quantity,Price,LowStockThreshold,SupplierID")] Item item)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    // Log stock movement after editing item
                    LogStockMovement(item, "Item Edited");

                    // Check for low stock and create an alert if necessary
                    CreateLowStockAlert(item);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
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

            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                // Log stock movement after deleting item
                LogStockMovement(item, "Item Deleted");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }

        // Helper function to log stock movement (add a stock movement record)
        private void LogStockMovement(Item item, string action)
        {
            var stockMovement = new StockMovement
            {
                ItemID = item.ItemID,
                UserID = GetCurrentUserSupplierID(), // Assuming the current user is updating the stock
                OldQuantity = item.Quantity,  // The stock before update
                NewQuantity = item.Quantity,  // The stock after update
                Action = action, // "Created", "Edited", "Deleted"
                MovementDate = DateTime.Now
            };

            _context.StockMovements.Add(stockMovement);
            _context.SaveChanges();
        }

        // Helper function to get the current user's SupplierID (for Suppliers)
        private int GetCurrentUserSupplierID()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.UserID.ToString() == userId);
            return user?.Role == UserRole.Supplier ? user.UserID : -1;
        }

        // Helper function to create low stock alert
        private void CreateLowStockAlert(Item item)
        {
            if (item.Quantity <= item.LowStockThreshold)
            {
                var alert = new Alert
                {
                    ItemID = item.ItemID,
                    Message = $"Stock is low for item {item.Name}. Please restock.",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Add(alert);
                _context.SaveChanges();
            }
        }
    }
}
