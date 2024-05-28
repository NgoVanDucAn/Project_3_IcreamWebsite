using C2108G1_Project_3.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C2108G1_Project_3.Models

{
    public class Orders : Base
    {
        public string? Name { get; set; }
        public string? IdentityUserId { get; set; }
        public virtual Users? Users { get; set; }
        public int? BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Books? Books { get; set; }
        public int? quantity { get; set; }
        public DateTime? orderDate { get; set; } = DateTime.Now;
        public string? PhoneNumber { get; set; }
        public string? shippingAddress { get; set; }
        public decimal? Cost { get; set; }
        public OrderStatus? OrderStatus { get; set; }

    }
}
