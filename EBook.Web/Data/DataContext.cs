using EBook.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EBook.Web.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasData(new Category
            {
                Id = 1,
                Name = "Test 1",
                CreatedDateTime = DateTime.Now,
                DisplayOrder = 5
            });
    }
}

