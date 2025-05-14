using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Data;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventorymanagement.Controllers
{
    [Authorize] // This ensures that only logged-in users can access these actions
    public class UsersController : Controller
    {
        private readonly InventoryContext _context;

        public UsersController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Users/Login
        [AllowAnonymous] // Allow anonymous access for login
        public IActionResult Login(string returnUrl = null)
        {
            // If the user is already authenticated, redirect to the Users Index page
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl ?? "/Users/Index");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [AllowAnonymous] // Allow anonymous access for login
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string role, string returnUrl = null)
        {
            // Validate the login credentials
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null) // If user is found and login is successful
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()) // Get role from the database
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Set a success message to be displayed after login
                TempData["Username"] = username;

                // Redirect to the returnUrl if provided, otherwise redirect to the Users Index page
                return Redirect(returnUrl ?? "/Dashboard/Index");
            }

            // If login fails, show an error message
            TempData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("Login");  // Ensure it redirects back to Login
        }

        // GET: Users/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            // Check the user role and provide relevant data
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userRole == "Manager")
            {
                return View(await _context.Users.ToListAsync()); // Full access to all users for Manager
            }
            else if (userRole == "WarehouseStaff")
            {
                // Warehouse staff shouldn't see the whole list, maybe just their own details
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID.ToString() == userId);
                return View(new List<User> { user }); // Show only their details
            }
            else if (userRole == "Supplier")
            {
                // Supplier shouldn't see the user list; you could adjust what they see here
                return RedirectToAction("AccessDenied", "Home"); // Redirect to access denied page
            }

            return RedirectToAction("AccessDenied", "Home"); // In case no role is found
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the current user is authorized to view this user's details
            var currentRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (currentRole != "Manager" && currentRole != "WarehouseStaff")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Manager")] // Only Managers can create users
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")] // Only Managers can create users
        public async Task<IActionResult> Create([Bind("UserID,Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Only allow Manager to edit the user role
            var currentRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (currentRole != "Manager")
            {
                return RedirectToAction("AccessDenied", "Home"); // Redirect if not a manager
            }

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,Password,Role")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            // Only allow Manager to delete users
            var currentRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (currentRole != "Manager")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        // A simple Access Denied page
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
