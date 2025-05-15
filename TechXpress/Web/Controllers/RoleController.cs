using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Get all Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // Get all Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Create Role - Get
        public IActionResult CreateRole()
        {
            return View();
        }

        // Create Role - Post
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole roleIdentity)
        {
            var roleName = roleIdentity.Name;
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("Name", "Role name is required.");
                return View(roleIdentity);
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                ModelState.AddModelError("Name", "Role already exists.");
                return View(roleIdentity);
            }

            var role = new IdentityRole(roleName)
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(roleIdentity);
        }

        // Delete Role
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = "Role not found";
                return RedirectToAction("Index");
            }

            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        // Assign Roles to User - Get
        [HttpGet]
        public async Task<IActionResult> AsignRolesToUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return RedirectToAction("Index");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            if (!roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
                roles = await _roleManager.Roles.ToListAsync();
            }

            var roleList = roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                UserRole = userRoles.Contains(r.Name),
            }).ToList();

            ViewBag.UserId = userId;
            ViewBag.UserName = user.UserName;
            ViewBag.FullName = user.FullName;

            return View(roleList);
        }

        // Assign Roles to User - Post
        [HttpPost]
        public async Task<IActionResult> AsignRolesToUser(string userId, string jsonRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrWhiteSpace(jsonRoles))
            {
                ModelState.AddModelError("Role", "Please select at least one role.");
                return View();
            }

            List<RoleViewModel> MyRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonRoles);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in MyRoles)
                {
                    if (userRoles.Contains(role.RoleName.Trim()) && !role.UserRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName.Trim());
                    }

                    if (!userRoles.Contains(role.RoleName.Trim()) && role.UserRole)
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName.Trim());
                    }
                }

                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("Role", "User id not valid");
                return View();
            }
        }
    }
}
