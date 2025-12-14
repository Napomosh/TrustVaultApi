using Microsoft.EntityFrameworkCore.Storage;

namespace TrustDrop.Common.Database;

public interface ITransactional
{
    public Task<IDbContextTransaction> BeginTransactionScopeAsync();
    public IDbContextTransaction BeginTransactionScope();
}