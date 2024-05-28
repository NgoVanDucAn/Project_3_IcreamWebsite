using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Repository
{
	
		public interface IRegisterUsersRepository : IBaseRepository<RegisterUsers>
		{
			Task<List<RegisterUsers>> GetRegisterUserssByKeyword(string keyword);

		}
		public class RegisterUsersRepository : BaseRepository<RegisterUsers>, IRegisterUsersRepository
		{
			private readonly ApplicationDbContext _context;
			public RegisterUsersRepository(ApplicationDbContext context, UserManager<Users> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
			{
				_context = context;
			}

			public async Task<List<RegisterUsers>> GetRegisterUserssByKeyword(string keyword)
			{
				var query = from st in _context.RegisterUsers.AsQueryable()
							where st.Users.UserName.ToLower().Contains(keyword.ToLower())
							select st;

				return await query.ToListAsync();
			}
        public async Task<bool> DeleteOrders(int? id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order != null)
                {
                    _context.Orders.Remove(order);
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
                return false;
            }
        }
    }
	
}
