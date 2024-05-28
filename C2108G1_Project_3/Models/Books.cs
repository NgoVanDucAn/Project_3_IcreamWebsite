using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Humanizer.In;

namespace C2108G1_Project_3.Models
{

    public class Books : Base
    {

        [Required]
        public string? bookName { get; set; }
        public string? bookDescription { get; set; }
        public string? author { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }
        public string? thumbnail { get; set; }
    }
}
