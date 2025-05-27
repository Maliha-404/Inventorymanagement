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
                return View(await items.ToListAsync());
            }
            else if (userRole == "Supplier")
            {
                var supplierItems = await items.Where(i => i.SupplierID == GetCurrentUserSupplierID()).ToListAsync();
                return View(supplierItems);
            }

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
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    LogStockMovement(item, "Item Created");
                    CreateLowStockAlert(item);
                    //return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError("", $"An error occurred while saving the item: {ex.Message}");
                    //ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
                    //return View(item);
                    Console.WriteLine(ex.Message);
                }
            }

            //ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
            //return View(item);
            return RedirectToAction(nameof(Index));

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
                    // Check if the stock is updated and handle accordingly
                    var existingItem = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.ItemID == item.ItemID);
                    if (existingItem != null && existingItem.Quantity != item.Quantity)
                    {
                        // If quantity is updated, log the stock movement
                        LogStockMovement(item, "Item Edited");
                    }

                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    // Handle the low stock alert after edit
                    CreateLowStockAlert(item);

                    // Redirect to Index after successful update
                    return RedirectToAction(nameof(Index));
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
            }

            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", item.SupplierID);
            return View(item); // If update fails, stay on the edit page
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
                LogStockMovement(item, "Item Deleted");
            }

            return RedirectToAction(nameof(Index)); // Redirect after deleting
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }
        // GET: Items/StockMovements
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> StockMovements()
        {
            // Retrieve all stock movements
            var stockMovements = await _context.StockMovements
                                                .Include(s => s.Item) // Include related Item data
                                                .Include(s => s.User) // Include related User data
                                                .ToListAsync();

            return View(stockMovements); // Pass stock movements to the view
        }

        // Helper function to log stock movement (add a stock movement record)
        private void LogStockMovement(Item item, string action)
        {
            var userId = GetCurrentUserSupplierID();
            Console.WriteLine($"UserID being passed: {userId}");  // Log the UserID

            if (userId == -1)
            {
                // Log an error message if the UserID is invalid
                Console.WriteLine("Error: Invalid UserID, cannot log stock movement.");
                return; // Exit if UserID is invalid
            }

            //var stockMovement = new StockMovement
            //{
            //    ItemID = item.ItemID,
            //    UserID = userId, // Ensure this value is valid
            //    OldQuantity = item.Quantity,
            //    NewQuantity = item.Quantity,
            //    Action = action,
            //    MovementDate = DateTime.Now
            //};
            var existingItem = _context.Items.FirstOrDefault(i => i.ItemID == item.ItemID);
            if (existingItem == null)
            {
                Console.WriteLine($"Error: Item with ID {item.ItemID} not found.");
                return;
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var stockMovement = new StockMovement
                    {
                        ItemID = item.ItemID,
                        UserID = userId,
                        OldQuantity = item.Quantity,
                        NewQuantity = item.Quantity,
                        Action = action,
                        MovementDate = DateTime.Now
                    };

                    _context.StockMovements.Add(stockMovement);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    Console.WriteLine("Error while saving stock movement: " + ex.Message);
                }
            }
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
            else
            {
                // Resolve the alert if stock is above the threshold
                var alert = _context.Alerts.FirstOrDefault(a => a.ItemID == item.ItemID && a.IsActive);
                if (alert != null)
                {
                    alert.IsActive = false;
                    _context.SaveChanges();
                }
            }
        }
    }
}
