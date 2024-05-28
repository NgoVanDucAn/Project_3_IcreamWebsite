using C2108G1_Project_3.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C2108G1_Project_3.Models
{

    public class Recipes : Base
    {

        [Required]
        public string? recipeName { get; set; }
        public string? introduce { get; set; }
        public string? recipeDescription { get; set; }
        public string? thumbnail { get; set; }
        public string? IdentityUserId { get; set; }
        public virtual Users? Users { get; set; }
        public int? flavorsId { get; set; }
        [ForeignKey(nameof(flavorsId))]
        public virtual Flavors? GetFlavors { get; set; }
        public bool? IsFree { get; set; }
        public IsTop? isTop { get; set; }
    }
}


