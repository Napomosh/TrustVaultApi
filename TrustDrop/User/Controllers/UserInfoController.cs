using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustDrop.Common.Result;
using TrustDrop.User.Bl;
using TrustDrop.User.Models;

namespace TrustDrop.User.Controllers;

[Route("api")]
[ApiController]
public class UserInfoController(IUserInfoBl userInfoBl) : ControllerBase
{
    private readonly IUserInfoBl _userInfoBl = userInfoBl;
    
    [Authorize]
    [Route("user-info")]
    public async Task<ActionResult> GetUserInfo(string login)
    {
        var userInfo = await _userInfoBl.GetUserInfoModel(login);
        return Ok(new JsonResult<UserInfoModel>(userInfo));
    }
}
