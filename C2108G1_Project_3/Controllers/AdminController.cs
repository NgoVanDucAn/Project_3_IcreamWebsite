using C2108G1_Project_3.Data;
using C2108G1_Project_3.Enum;
using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace C2108G1_Project_3.Controllers
{

    [Authorize(Roles = "ADMIN, MANAGER")]

    public class AdminController : Controller
    {
        private readonly IUsersRepository<Users> _usersRepository;
        protected readonly UserManager<Users> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IUsersRepository<Users> usersRepository, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult IndexBooks()
		{
			return View();
		}
        public IActionResult CreateOrUpdateBook(int id)
        {
            return View(id);
        }
        public IActionResult BooksRecipesAndFlavors()
        {
            return View();
        }
        public IActionResult CreateOrUpdateFlavor(int id)
        {
            return View(id);
        }
        public IActionResult IndexOrders()
        {
            return View();
        }
        public IActionResult CreateOrUpdateOrder(int id)
        {
            return View(id);
        }
        public IActionResult IndexRecipes()
        {
            return View();
        }
        public IActionResult CreateOrUpdateRecipe(int id)
        {
            return View(id);
        }
        public IActionResult IndexUsers()
        {
            return View();
        }
       
        [HttpGet("[controller]/[action]/{id?}")]
        public async Task<IActionResult> viewSetRoles(string id)
        {
            TempData["IdOfuser"] = id;
            var x = await _userManager.FindByIdAsync(id);
            var dsAllQuyen = await _roleManager.Roles.ToListAsync();
            if (x != null)
            {
                var userRole = await _userManager.GetRolesAsync(x);
                ViewBag.UserRole = userRole.ToList();
                ViewBag.userId = id;
                //var userRole = await _userManager.GetUsersInRoleAsync(id);
            }
            return PartialView(dsAllQuyen);
        }
        public IActionResult UpdateUsers(string Id)
        {
            TempData["GetUserId"] = Id;
            return View("UpdateUsers");
        }
        public IActionResult Error404()
        {
            return View();
        }

    }
}
