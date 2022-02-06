using EBook.Models;

namespace EBook.DataAccess.Interfaces;
public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
    void Save();
}
