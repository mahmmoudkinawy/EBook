namespace EBook.DataAccess.Interfaces;
public interface ICoverTypeRepository : IRepository<CoverType>
{
    void Update(CoverType coverType);
}

