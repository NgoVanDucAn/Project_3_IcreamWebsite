using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Mvc;

namespace C2108G1_Project_3.Controllers
{
    [ApiController]
    public class FlavorsController : APIBaseController<Flavors>
    {
       
            private readonly IFlavorsRepostiory _flavorsRepostiory;


            public FlavorsController(IBaseRepository<Flavors> repository, IFlavorsRepostiory flavorsRepostiory) : base(repository)
            {
                _flavorsRepostiory = flavorsRepostiory;

            }


        [HttpGet("[controller]/[action]")]
        public async Task<List<Flavors>> GetFlavorsByKeyword(string keyword)
        {
            var result = await _flavorsRepostiory.GetFlavorsByKeyword(keyword);
            return result;
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> CreateOrUpdateFlavor([FromForm] int? id, [FromForm] Flavors flavors, IFormFile thumbnail)
        {
            var result = await _flavorsRepostiory.CreateOrUpdateFlavor(id, flavors, thumbnail);

            if (result)
            {
                return RedirectToAction("BooksRecipesAndFlavors", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }
        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> DeleteFlavor([FromForm] int? id)
        {
            var result = await _flavorsRepostiory.DeleteFlavor(id);

            if (result)
            {
                return RedirectToAction("BooksRecipesAndFlavors", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi xóa sách
            }
        }
    }
}
