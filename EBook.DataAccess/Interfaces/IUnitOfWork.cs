namespace EBook.DataAccess.Interfaces;
public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    void Save();
}

