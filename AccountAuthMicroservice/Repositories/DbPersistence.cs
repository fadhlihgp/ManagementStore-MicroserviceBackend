using AccountAuthMicroservice.Context;
using AccountAuthMicroservice.Repositories.Interface;

namespace AccountAuthMicroservice.Repositories;

public class DbPersistence : IPersistence
{
    private AppDbContext _context;

    public DbPersistence(AppDbContext context)
    {
        _context = context;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}