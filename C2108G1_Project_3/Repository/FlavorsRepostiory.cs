using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Repository
{
    
        public interface IFlavorsRepostiory : IBaseRepository<Flavors>
        {
        Task<List<Flavors>> GetFlavorsByKeyword(string keyword);
        Task<bool> CreateOrUpdateFlavor(int? id, Flavors req, IFormFile thumbnail);
        Task<bool> DeleteFlavor(int? id);
        Task<List<Flavors>> GetFlavorsById(int? id);
        }
        public class FlavorsRepository : BaseRepository<Flavors>, IFlavorsRepostiory
    {
            private readonly ApplicationDbContext _context;
            public FlavorsRepository(ApplicationDbContext context, UserManager<Users> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
            {
                _context = context;
            }

            public async Task<List<Flavors>> GetFlavorsByKeyword(string keyword)
            {
                var query = from st in _context.Flavors.AsQueryable()
                            where st.Name.ToLower().Contains(keyword.ToLower())
                            select st;

                return await query.ToListAsync();
            }



            public async Task<bool> CreateOrUpdateFlavor(int? id, Flavors req, IFormFile thumbnail)
            {
                try
                {
                    var existingFlavor = await _context.Flavors.FindAsync(id);

                    if (existingFlavor == null)
                    {
                        // Tạo sách mới nếu sách không tồn tại
                        var newFlavor = new Flavors
                        {
                            Name = req.Name,
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

                            newFlavor.thumbnail = uploadImgName;
                        }

                        _context.Flavors.Add(newFlavor);
                    }
                    else
                    {
                        // Cập nhật thông tin sách nếu sách đã tồn tại
                        existingFlavor.Name = req.Name;
                        existingFlavor.UpdatedUser = req.UpdatedUser = currentUserId;
                        existingFlavor.UpdatedTime = req.UpdatedTime = DateTime.Now;
                        existingFlavor.IsDeleted = req.IsDeleted;

                        if (thumbnail != null)
                        {
                            string uploadImgName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", uploadImgName);

                            // Xóa các tệp tin ảnh cũ
                            foreach (var fileName in existingFlavor.thumbnail.Split(';'))
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

                            existingFlavor.thumbnail = uploadImgName;
                        }

                        _context.Update(existingFlavor);
                    }

                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false; // Xảy ra lỗi khi tạo hoặc cập nhật sách
                }
            }


            public async Task<bool> DeleteFlavor(int? id)
            {
                try
                {
                    var flavor = await _context.Flavors.FindAsync(id);

                    if (flavor != null)
                    {
                        // Xóa tệp tin ảnh của sách
                        if (flavor.thumbnail != null)
                        {
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", flavor.thumbnail);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        _context.Flavors.Remove(flavor);
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

        public async Task<List<Flavors>> GetFlavorsById(int? id)
        {
            if (id == null)
            {
                return null; // Trả về null nếu id không được cung cấp
            }

            var flavors = await _context.Flavors
                .Where(f => f.id == id)
                .ToListAsync();

            return flavors;
        }
    }

}
