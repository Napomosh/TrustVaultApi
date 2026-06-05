using TrustDrop.Auth.Dal;
using TrustDrop.Common.Error;
using TrustDrop.Common.Result;
using TrustDrop.User.Dal;
using TrustDrop.User.Models;

namespace TrustDrop.User.Bl;

public class UserInfoBl(IUserInfoDal userInfoDal, IAuthDal authDal) : IUserInfoBl
{
    private readonly IUserInfoDal _userInfoDal = userInfoDal;
    private readonly IAuthDal _authDal = authDal;
    
    public async Task<Result<UserInfoModel>> GetUserInfoModel(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result<UserInfoModel>.Failure(ErrorCode.NotFound, GeneralError.NOT_FOUND);
        
        var userInfo = await _userInfoDal.GetUserInfoModel(userId);

        return userInfo is not null 
            ? Result<UserInfoModel>.Success(userInfo) 
            : Result<UserInfoModel>.Failure(ErrorCode.NotFound, GeneralError.NOT_FOUND);
    }

    public async Task<Result<UserInfoModel>> GetUserInfoModel(string userLogin)
    {
        var userModel = await _authDal.GetUser(userLogin);

        return await GetUserInfoModel(userModel?.Id ?? Guid.Empty);
    }
}