using Microsoft.AspNetCore.Identity;
using C2108G1_Project_3.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Humanizer.In;

namespace C2108G1_Project_3.Models
{
    public class Users : IdentityUser
    {
		public string? fullname { get; set; }
	}
}



