using Microsoft.EntityFrameworkCore;
using bloomApiProject.Models;

namespace bloomApiProject.Data{

public class bloomApiProjectDbContext: DbContext{
    public bloomApiProjectDbContext(DbContextOptions<bloomApiProjectDbContext> options):base (options){

    }
    public DbSet<Products> Products { get; set; }
    public DbSet<Users> users{get;set;}
    public DbSet<CartItem> CartItems{get;set;}

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().ToTable("products");
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<CartItem>().ToTable("carts");

        }

    

}
}