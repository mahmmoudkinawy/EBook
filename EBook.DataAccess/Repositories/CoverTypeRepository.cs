namespace EBook.DataAccess.Repositories;
public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
{
    private readonly DataContext _context;
    public CoverTypeRepository(DataContext context) : base(context)
        => _context = context;

    public void Update(CoverType coverType) => _context.CoverTypes.Update(coverType);

}

