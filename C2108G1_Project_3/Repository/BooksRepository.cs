using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace C2108G1_Project_3.Repository
{
    

    public interface IBooksRepository : IBaseRepository<Books>
    {

        Task<List<Books>> GetBooksByKeyword(string keyword);
        Task<bool> CreateOrUpdateBook(int? id, Books req, IFormFile thumbnail);
        Task<bool> DeleteBook(int? id);

    }
    public class BooksRepository : BaseRepository<Books>, IBooksRepository
    {
            private readonly ApplicationDbContext _context;
            public BooksRepository(ApplicationDbContext context, UserManager<Users> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
            {
                _context = context;
            }

            public async Task<List<Books>> GetBooksByKeyword(string keyword)
            {
                var query = from st in _context.Books.AsQueryable()
                            where st.bookName.ToLower().Contains(keyword.ToLower()) ||
                                  st.author.ToLower().Contains(keyword.ToLower())
                            select st;

                return await query.ToListAsync();
            }
        


        public async Task<bool> CreateOrUpdateBook(int? id, Books req, IFormFile thumbnail)
        {
            try
            {
                var existingBook = await _context.Books.FindAsync(id);

                if (existingBook == null)
                {
                    // Tạo sách mới nếu sách không tồn tại
                    var newBook = new Books
                    {
                        bookName = req.bookName,
                        author = req.author,
                        bookDescription = req.bookDescription,
                        quantity = req.quantity,
                        price = req.price,
                        CreatedUser = req.CreatedUser = currentUserId,
                        CreatedTime = req.CreatedTime = DateTime.Now, 
                        IsDeleted = req.IsDeleted = false
                    };

                    if (thumbnail != null)
                    {
                        string uploadImgName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", uploadImgName);

                        using (var StreamUploadFile = new FileStream(savePath, FileMode.CreateNew))
                        {
                            thumbnail.CopyTo(StreamUploadFile);
                        }

                        newBook.thumbnail = uploadImgName;
                    }

                    _context.Books.Add(newBook);
                }
                else
                {
                    // Cập nhật thông tin sách nếu sách đã tồn tại
                    existingBook.bookName = req.bookName;
                    existingBook.author = req.author;
                    existingBook.bookDescription = req.bookDescription;
                    existingBook.quantity = req.quantity;
                    existingBook.price = req.price;
                    existingBook.UpdatedUser = req.UpdatedUser = currentUserId;
                    existingBook.UpdatedTime = req.UpdatedTime = DateTime.Now;
                    existingBook.IsDeleted= req.IsDeleted;

                    if (thumbnail != null)
                    {
                        string uploadImgName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", uploadImgName);

                        // Xóa các tệp tin ảnh cũ
                        foreach (var fileName in existingBook.thumbnail.Split(';'))
                        {
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", fileName);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        using (var StreamUploadFile = new FileStream(savePath, FileMode.CreateNew))
                        {
                            thumbnail.CopyTo(StreamUploadFile);
                        }
                        
                        existingBook.thumbnail = uploadImgName;
                    }

                    _context.Update(existingBook);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }


        public async Task<bool> DeleteBook(int? id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book != null)
                {
                    // Xóa tệp tin ảnh của sách
                    if (book.thumbnail != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", book.thumbnail);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false; // Không tìm thấy cuốn sách cần xóa
                }
            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi khi xóa sách
            }
        }
    }


}
