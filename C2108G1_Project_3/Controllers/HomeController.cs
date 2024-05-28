using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace C2108G1_Project_3.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersRepository<Users> _usersRepository;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IRegisterUsersRepository _registerUsersRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, IUsersRepository<Users> usersRepository, UserManager<Users> userManager, SignInManager<Users> signInManager, IRegisterUsersRepository registerUsersRepository,IHttpContextAccessor contextAccessor, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _usersRepository = usersRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _registerUsersRepository= registerUsersRepository;
            _contextAccessor = contextAccessor;
            _applicationDbContext = applicationDbContext;
            
        }

        
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult();

            if (user != null)
            {
                var query = from u in _applicationDbContext.Users
                            join ur in _applicationDbContext.RegisterUsers on u.Id equals ur.IdentityUserId
                            where u.Id == user.Id
                            orderby  ur.dueDate descending
                            select ur.dueDate;
                
                var expirationDate = query.FirstOrDefault();
                if (expirationDate != null && DateTime.Now > expirationDate)
                {
                    ViewBag.MembershipExpirationMessage = "Your membership has expired. Please renew your membership.";
                }
            }
            return View();

        }
        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult Recipes()
        {
            return View();
        }
        public async Task<IActionResult> UserDetails(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            
            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return NotFound();
            }
           
            // Truyền thông tin người dùng vào view
            return View(user);
           
        }
        
        public IActionResult FeedBack()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Subscribe()
        {
            return View();
        }
        public IActionResult Books()
        {
            return View();
        }
        public IActionResult RecipeDetail(int id)
        {
            return View(id);
        }
        public IActionResult BookDetail(int id)
        {
            return View(id);
        }

        
        public IActionResult Order(int id, int quantity)
        {
            TempData["quantity"] = quantity;
            return View(id);
        }
        
        public IActionResult CreateOrupdateRecipesForUser(int id)
        {
            var getuserrole = _contextAccessor.HttpContext.User.IsInRole("SUBUSERS");
            if (!getuserrole)
            {
                
                return RedirectToAction("Subscribe", "Home");
            }
            
            return View(id);
            
           
        }
        
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult indexBookTest()
        {
            return View();
        }
        public IActionResult OrderTest()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}