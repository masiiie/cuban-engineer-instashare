namespace InstaShare>Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IFileRepository _fileRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}