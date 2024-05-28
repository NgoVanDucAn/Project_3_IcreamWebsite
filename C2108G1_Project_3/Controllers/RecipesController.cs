using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Mvc;

namespace C2108G1_Project_3.Controllers
{

    [ApiController]
    public class RecipesController : APIBaseController<Recipes>
    {

        private readonly IRecipesRepository _recipesRepository;


        public RecipesController(IBaseRepository<Recipes> repository, IRecipesRepository recipesRepository) : base(repository)
        {
            _recipesRepository = recipesRepository;

        }

        [HttpGet("[controller]/[action]")]
        public async Task<List<Recipes>> Search(string keyword)
        {
            var result = await _recipesRepository.Search(keyword);
            return result;
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> CreateOrUpdateRecipe([FromForm] int? id, [FromForm] Recipes recipes, IFormFile thumbnail)
        {


            var result = await _recipesRepository.CreateOrUpdateRecipe(id, recipes, thumbnail);

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
        public async Task<IActionResult> CreateOrupdateRecipesForUser([FromForm] int? id, [FromForm] Recipes recipes, IFormFile thumbnail)
        {


            var result = await _recipesRepository.CreateOrupdateRecipesForUser(id, recipes, thumbnail);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                return BadRequest(); // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> DeleteRecipe([FromForm] int? id)
        {
            var result = await _recipesRepository.DeleteRecipe(id);

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
