using Microsoft.EntityFrameworkCore;
using CombineSample.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace CombineSample.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>{
    public virtual DbSet<Employee>  Employee_Details {get;set;}
    public virtual DbSet<Manager>  Manager_Details {get;set;}
    
    public virtual DbSet<AddEmployee>  AddEmployee_Details {get;set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){
    }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){

        base.OnModelCreating(modelBuilder);
           //primary key for the entities
            modelBuilder.Entity<Employee>()
            .HasKey(u => u.employeeId);
            modelBuilder.Entity<AddEmployee>()
            .HasKey(u => u.employeeId);
            modelBuilder.Entity<Manager>()
            .HasKey(u => u.managerId);
    }
    public static implicit operator UserManager<object>(ApplicationDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}