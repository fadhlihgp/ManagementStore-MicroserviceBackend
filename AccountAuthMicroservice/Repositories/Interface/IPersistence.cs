namespace AccountAuthMicroservice.Repositories.Interface;

public interface IPersistence
{
    void SaveChanges();
    Task SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}