using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace C2108G1_Project_3.Repository
{

    public interface IRecipesRepository : IBaseRepository<Recipes>
    {

        Task<List<Recipes>> Search(string keyword);
        Task<bool> CreateOrUpdateRecipe(int? id, Recipes req, IFormFile thumbnail);
        Task<bool> DeleteRecipe(int? id);
        Task<List<Recipes>> GetRecipesWithSubscription();
        Task<List<Recipes?>> GetRecipesByUserId(string userId);
        Task<bool> CreateOrupdateRecipesForUser(int? id, Recipes req, IFormFile thumbnail);
    }
    public class RecipesRepository : BaseRepository<Recipes>, IRecipesRepository
    {
        private readonly ApplicationDbContext _context;
        public RecipesRepository(ApplicationDbContext context, UserManager<Users> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<List<Recipes>> Search(string keyword)
        {
            var result = _context.Recipes.AsQueryable();
           
            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(r => r.recipeName.ToLower().Contains(keyword.ToLower()));
            }
            

            return await result.ToListAsync();

        }

        public async Task<bool> CreateOrUpdateRecipe(int? id, Recipes req, IFormFile thumbnail)
        {
            try
            {
                var existingRecipe = await _context.Recipes.FindAsync(id);

                if (existingRecipe == null)
                {
                    // Tạo sách mới nếu sách không tồn tại
                    var newRecipe = new Recipes
                    {
                        recipeName = req.recipeName,
                        introduce = req.introduce,
                        recipeDescription = req.recipeDescription,
                        IdentityUserId = req.IdentityUserId,
                        flavorsId = req.flavorsId,
                        isTop = req.isTop,
                        IsFree = req.IsFree,
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

                        newRecipe.thumbnail = uploadImgName;
                    }

                    _context.Recipes.Add(newRecipe);
                }
                else
                {
                    // Cập nhật thông tin sách nếu sách đã tồn tại
                    existingRecipe.recipeName = req.recipeName;
                    existingRecipe.introduce = req.introduce;
                    existingRecipe.recipeDescription = req.recipeDescription;
                    existingRecipe.IdentityUserId= req.IdentityUserId;
                    existingRecipe.flavorsId = req.flavorsId;
                    existingRecipe.isTop = req.isTop;
                    existingRecipe.IsFree = req.IsFree;
                    existingRecipe.UpdatedUser = req.UpdatedUser = currentUserId;
                    existingRecipe.UpdatedTime = req.UpdatedTime = DateTime.Now;
                    existingRecipe.IsDeleted = req.IsDeleted;

                    if (thumbnail != null)
                    {
                        string uploadImgName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", uploadImgName);

                        // Xóa các tệp tin ảnh cũ
                        foreach (var fileName in existingRecipe.thumbnail.Split(';'))
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

                        existingRecipe.thumbnail = uploadImgName;
                    }

                    _context.Update(existingRecipe);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }
        

        public async Task<bool> DeleteRecipe(int? id)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(id);

                if (recipe != null)
                {
                    // Xóa tệp tin ảnh của sách
                    if (recipe.thumbnail != null)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", recipe.thumbnail);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.Recipes.Remove(recipe);
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
        public async Task<List<Recipes?>> GetRecipesByUserId(string userId)
        {
            var recipes = await _context.Recipes
                .Where(r => r.IdentityUserId == userId)
                .ToListAsync();
           
                return recipes;
            
        }

        public async Task<List<Recipes>> GetRecipesWithSubscription()
        {
            var result = await GetAllAsync();
            var getuserrole = _contextAccessor.HttpContext.User.IsInRole("SUBUSERS");
            
            if (string.IsNullOrEmpty(currentUserId))
            {
                if (!getuserrole)
                {
                    result = result.Where(r => r.IsFree == true || r.IsFree == false).ToList();
                }
                result = result.Where(r => r.IsFree == true).ToList();
            }
            else
            {
                result = result.Where(r => r.IsFree == true).ToList();
            }
            return result;
        }

        public async Task<bool> CreateOrupdateRecipesForUser(int? id, Recipes req, IFormFile thumbnail)
        {
            try
            {
                var existingRecipe = await _context.Recipes.FindAsync(id);

                if (existingRecipe == null)
                {
                    // Tạo sách mới nếu sách không tồn tại
                    var newRecipe = new Recipes
                    {
                        recipeName = req.recipeName,
                        introduce = req.introduce,
                        recipeDescription = req.recipeDescription,
                        IdentityUserId = req.IdentityUserId = currentUserId,
                        flavorsId = req.flavorsId,
                        isTop = req.isTop = Enum.IsTop.Non,
                        IsFree = req.IsFree = true,
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

                        newRecipe.thumbnail = uploadImgName;
                    }

                    _context.Recipes.Add(newRecipe);
                }
                else
                {
                    // Cập nhật thông tin sách nếu sách đã tồn tại
                    existingRecipe.recipeName = req.recipeName;
                    existingRecipe.introduce = req.introduce;
                    existingRecipe.recipeDescription = req.recipeDescription;
                    existingRecipe.IdentityUserId = req.IdentityUserId = currentUserId;
                    existingRecipe.flavorsId = req.flavorsId;
                    existingRecipe.isTop = req.isTop = Enum.IsTop.Non;
                    existingRecipe.IsFree = req.IsFree = true;
                    existingRecipe.UpdatedUser = req.UpdatedUser = currentUserId;
                    existingRecipe.UpdatedTime = req.UpdatedTime = DateTime.Now;
                    existingRecipe.IsDeleted = req.IsDeleted;

                    if (thumbnail != null)
                    {
                        string uploadImgName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                        string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadImg", uploadImgName);

                        // Xóa các tệp tin ảnh cũ
                        foreach (var fileName in existingRecipe.thumbnail.Split(';'))
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

                        existingRecipe.thumbnail = uploadImgName;
                    }

                    _context.Update(existingRecipe);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }
    }

}
