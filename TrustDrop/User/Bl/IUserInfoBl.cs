using TrustDrop.Common.Result;
using TrustDrop.User.Models;

namespace TrustDrop.User.Bl;

public interface IUserInfoBl
{
    public Task<Result<UserInfoModel>> GetUserInfoModel(Guid userId);
    public Task<Result<UserInfoModel>> GetUserInfoModel(string userLogin);
}