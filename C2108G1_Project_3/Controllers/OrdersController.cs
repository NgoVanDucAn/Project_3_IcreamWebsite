using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Controllers
{
    [ApiController]
    public class OrdersController : APIBaseController<Orders>
    {
        private readonly IOrdersrepository _ordersRepostiory;


        public OrdersController(IBaseRepository<Orders> repository, IOrdersrepository ordersRepostiory) : base(repository)
        {
            _ordersRepostiory = ordersRepostiory;

        }

        [HttpGet("[controller]/[action]")]
        public async Task<List<Orders>> GetOrdersByKeyword(string keyword)
        {
            var result = await _ordersRepostiory.GetOrdersByKeyword(keyword);
            return result;
        }

        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> CreateOrUpdateOrders([FromForm] int? id, [FromForm] Orders orders)
        {
            

            var result = await _ordersRepostiory.CreateOrUpdateOrders(id, orders);

            if (result)
            {
                return RedirectToAction("IndexOrders", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi tạo hoặc cập nhật sách
            }
        }
        [HttpPost("[controller]/[action]/{id?}")]
        public async Task<IActionResult> DeleteOreders([FromForm] int? id)
        {
            var result = await _ordersRepostiory.DeleteOrders(id);

            if (result)
            {
                return RedirectToAction("IndexOrders", "Admin");
            }
            else
            {
                return BadRequest(); // Xảy ra lỗi khi xóa sách
            }
        }

    }
}
