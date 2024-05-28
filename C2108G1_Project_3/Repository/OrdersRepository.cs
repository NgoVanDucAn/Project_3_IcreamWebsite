using C2108G1_Project_3.Data;
using C2108G1_Project_3.Enum;
using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Odbc;

namespace C2108G1_Project_3.Repository
{

    public interface IOrdersrepository : IBaseRepository<Orders>
    {
        Task<List<Orders>> GetOrdersByKeyword(string keyword);
        Task<bool> CreateOrUpdateOrders(int? id, Orders req);
        Task<bool> DeleteOrders(int? id);

    }
    public class OrdersRepository : BaseRepository<Orders>, IOrdersrepository
    {
        private readonly ApplicationDbContext _context;
        public OrdersRepository(ApplicationDbContext context, UserManager<Users> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<List<Orders>> GetOrdersByKeyword(string keyword)
        {

            var result = _context.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(r => r.Name.ToLower().Contains(keyword.ToLower()));
            }
            result = result.Include(r => r.Books);


            return await result.ToListAsync();
        }


        public async Task<bool> CreateOrUpdateOrders(int? id, Orders req)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(id);
                if( existingOrder == null)
                {
                    
                    var newOrder = new Orders
                    {
                        Name = req.Name,
                        IdentityUserId = req.IdentityUserId = currentUserId,
                        BookId= req.BookId,
                        orderDate = req.orderDate,
                        PhoneNumber = req.PhoneNumber,
                        shippingAddress = req.shippingAddress,
                        Cost = req.Cost,
                        OrderStatus = req.OrderStatus,
                        CreatedUser = req.CreatedUser = currentUserId,
                        CreatedTime = req.CreatedTime = DateTime.Now,
                        IsDeleted = req.IsDeleted = false
                    };

                    _context.Orders.Add(newOrder);

                }


                else
                {
                    
                    existingOrder.Name = req.Name;
                    existingOrder.IdentityUserId = req.IdentityUserId;
                    existingOrder.BookId = req.BookId;
                    existingOrder.orderDate = req.orderDate;
                    existingOrder.PhoneNumber = req.PhoneNumber;
                    existingOrder.shippingAddress = req.shippingAddress;
                    existingOrder.Cost = req.Cost;
                    existingOrder.OrderStatus = req.OrderStatus;
                    existingOrder.UpdatedUser = req.UpdatedUser = currentUserId;
                    existingOrder.UpdatedTime = req.UpdatedTime = DateTime.Now;
                    existingOrder.IsDeleted = req.IsDeleted;

                    _context.Update(existingOrder);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false; 
            }
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
