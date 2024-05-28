using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Repository
{
    public interface IUsersRepository<T> where T : Users
    {
        Task<List<T>> GetAllUsers();
        Task<T> GetByIdAsync(string id);
        Task<bool> UpdateUsers(string? Id, T entity);
        Task<bool> DeleteUser(string? Id);
        Task<List<Users>> GetUsersByKeyword(string keyword);
    }

    public class UsersRepository<T> : IUsersRepository<T> where T : Users
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<T> _userManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DbSet<T> _dbSet;
        public UsersRepository(ApplicationDbContext context, UserManager<T> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _dbSet = context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<List<Users>> GetUsersByKeyword(string keyword)
        {
            var query = from st in _context.Users.AsQueryable()
                        where st.UserName.ToLower().Contains(keyword.ToLower())
                        select st;

            return await query.ToListAsync();
        }
        

        public async Task<List<T>> GetAllUsers()
        {

            var result = await _dbSet.ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var result = await _dbSet.FindAsync(id);
            return result;
        }

       



        public async Task<bool> UpdateUsers(string? Id, T entity)
        {
            var existingUser = await _context.Users.FindAsync(Id);
            try {
                if (entity != null)
                {
                    existingUser.UserName = entity.UserName;
                    existingUser.NormalizedUserName = entity.UserName.ToUpper();
                    existingUser.NormalizedEmail = entity.UserName.ToUpper();
                    existingUser.Email = entity.UserName;
                    existingUser.PasswordHash = entity.PasswordHash;
                    existingUser.fullname = entity.fullname;



                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi
            }
                
        }


        public async Task<bool> DeleteUser(string? Id)
        {
            try
            {
                var users = await _context.Users.FindAsync(Id);

                if (users != null)
                {
                    _context.Users.Remove(users);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false; // Không tìm thấy
                }
            }
            catch (Exception)
            {
                return false; // Xảy ra lỗi
            }
        }
    }
}