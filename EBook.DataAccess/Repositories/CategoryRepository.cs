using EBook.DataAccess.Interfaces;
using EBook.DataAccess.Migrations;
using EBook.Models;

namespace EBook.DataAccess.Repositories;

//ICategoryRepository  : this is will be the interface for category and for the generic interface
//Repository<Category> : implementation for the generic repository which inherets from IRepository<T>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly DataContext _context;
    public CategoryRepository(DataContext context) : base(context) => _context = context;
    public void Update(Category category) => _context.Categories.Update(category);
    public void Save() => _context.SaveChanges();
}

