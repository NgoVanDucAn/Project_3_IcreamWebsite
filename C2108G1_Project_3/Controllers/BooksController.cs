using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace C2108G1_Project_3.Controllers
{
    [ApiController]
    public class BooksController : APIBaseController<Books>
    {

        private readonly IBooksRepository _booksRepository;
       

        public BooksController(IBaseRepository<Books> repository, IBooksRepository booksRepository): base(repository)
        {
            _booksRepository = booksRepository;
            
        }

        [HttpGet("[controller]/[action]")]
        public async Task<List<Books>>GetBooksByKeyword(string keyword)
        {
            var result = await _booksRepository.GetBooksByKeyword(keyword);
            return result;
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> CreateOrUpdateBook([FromForm] int? id, [FromForm] Books book, IFormFile thumbnail)
        {
            var result = await _booksRepository.CreateOrUpdateBook(id, book, thumbnail);

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
        public async Task<IActionResult> DeleteBook([FromForm] int? id)
        {
            var result = await _booksRepository.DeleteBook(id);

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
