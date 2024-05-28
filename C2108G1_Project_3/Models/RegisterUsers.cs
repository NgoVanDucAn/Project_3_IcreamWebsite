using C2108G1_Project_3.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace C2108G1_Project_3.Models
{
	public class RegisterUsers:Base
	{

		[Required]
		public DateTime? registrationDate { get; set; } = DateTime.Now;
		public DateTime? dueDate { get; set; }
        
        public MembershipType? membershipType { get; set; }
     
        

        public string? IdentityUserId { get; set; }
        public virtual Users? Users { get; set; }
    }
}
