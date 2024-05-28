using C2108G1_Project_3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace C2108G1_Project_3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Flavors> Flavors { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Recipes> Recipes { get; set; }
        public virtual DbSet<RegisterUsers> RegisterUsers { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegisterUsers>()
                .HasOne(e => e.Users)
                .WithMany()
                .HasForeignKey(e => e.IdentityUserId);

            modelBuilder.Entity<Orders>()
                .HasOne(e => e.Users)
                .WithMany()
                .HasForeignKey(e => e.IdentityUserId);
            modelBuilder.Entity<Recipes>()
                .HasOne(e => e.Users)
                .WithMany()
                .HasForeignKey(e => e.IdentityUserId);
        }
       
    }
    
}