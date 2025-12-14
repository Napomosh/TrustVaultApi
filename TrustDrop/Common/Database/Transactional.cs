using Microsoft.EntityFrameworkCore.Storage;

namespace TrustDrop.Common.Database;

public class Transactional(AppDbContext _dbContext) : ITransactional
{
    private readonly AppDbContext _dbContext = _dbContext;
    
    public async Task<IDbContextTransaction> BeginTransactionScopeAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public IDbContextTransaction BeginTransactionScope()
    {
        return _dbContext.Database.BeginTransaction();
    }
}