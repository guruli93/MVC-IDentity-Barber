using System.Reflection;
using Domain.Booking;
using Domain.Image;
using Domain.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbContext;

public class DbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Booking> Booking { get; set; }
}

