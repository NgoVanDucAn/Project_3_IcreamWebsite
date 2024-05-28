using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;

using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Controllers
{

    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository<Users> _usersRepository;
        protected readonly UserManager<Users> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IUsersRepository<Users> usersRepository, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        [HttpGet("[controller]/[action]")]
        public async Task<List<Users>> GetUsersByKeyword(string keyword)
        {
            var result = await _usersRepository.GetUsersByKeyword(keyword);
            return result;
        }

        //public async Task<IActionResult> ViewSetRoles(string id)
        //{
        //    var userDuocChon = await _userManager.FindByIdAsync(id);

        //    var dsAllQuyen = await _roleManager.Roles.ToArrayAsync();
        //    if (userDuocChon != null)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(userDuocChon);
        //        ViewBag.userRoles = userRoles.ToList();
        //    }

        //    return View(dsAllQuyen);
        //}
       

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> DoUpdateRole([FromForm]List<String> arrRole)
        {
            var cuId = TempData["IdOfuser"];
            var user = await _userManager.FindByIdAsync(cuId.ToString());
            var dsAllQuyen = await _roleManager.Roles.ToListAsync();
            var arrQuyen = dsAllQuyen.Select(x => x.Name);
            
            if (user != null)
            {
                foreach (var quyen in arrQuyen)
                {
                    await _userManager.RemoveFromRoleAsync(user, quyen);
                }
                if (arrRole.Count > 0)
                {
                    foreach (var item in arrRole)
                    {
                        await _userManager.AddToRoleAsync(user, item);
                    }
                }
                return RedirectToAction("IndexUsers", "Admin");
            }
            return BadRequest();

        }
        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Roling()
        {
            var x = new dbSeedRole(_roleManager);
            await x.RoleData();
            return Ok("SUcced");
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> UserDetails(string id)
        {
            
            // Lấy thông tin chi tiết của người dùng với id được truyền vào
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return NotFound();
            }

            // Truyền thông tin người dùng vào view
            return RedirectToAction("UserDetails", "Home");
        }



        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> UpdateUsers([FromForm][Bind("UserName, PasswordHash, fullname")] Users users)
        {
            var Id = TempData["GetUserId"].ToString();
            var result = await _usersRepository.UpdateUsers(Id, users);

            if (result)
            {
                return RedirectToAction("IndexUsers", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }
        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> DeleteUser([FromForm] string? id)
        {
            
            var result = await _usersRepository.DeleteUser(id);
           
            if (result)
            {
                return RedirectToAction("IndexUsers", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi xóa sách
            }
        }
    }
}
    