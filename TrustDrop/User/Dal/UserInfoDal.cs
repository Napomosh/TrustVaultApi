using Microsoft.EntityFrameworkCore;
using TrustDrop.Common.Database;
using TrustDrop.User.Models;

namespace TrustDrop.User.Dal;

public class UserInfoDal(AppDbContext dbContext) : IUserInfoDal
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<UserInfoModel?> GetUserInfoModel(Guid userId)
    {
        var userInfo = await _dbContext.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoModel
            {
                UserName = u.Username,
                Email = u.Email,
                Organizations = u.UserTenantRoles.Select(role => role.Tenant.Name).ToList()
            }).FirstOrDefaultAsync();

        return userInfo;
    }
}