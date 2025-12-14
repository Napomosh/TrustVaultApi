using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustDrop.Auth.Bl;
using TrustDrop.Common.Error;
using TrustDrop.Common.Result;
using TrustDrop.Common.Result.Auth;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Controllers;

[Route("api")]
[ApiController]
public class AuthController(IAuthBl _authBl) : ControllerBase
{
    private readonly IAuthBl _authBl = _authBl;
    
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] UserViewModel parameters)
    {
        var result = await _authBl.RegisterUser(parameters.Login, parameters.Password, parameters.Email ?? string.Empty);
        if (result.IsSuccess)
            return Ok();
        if (result.Code == ErrorCode.Conflict)
            return Conflict(new JsonResult<bool>(result));
        return BadRequest();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] UserViewModel parameters)
    {
        var result = await _authBl.LoginUser(parameters.Login, parameters.Password);
        if (result.IsSuccess)
            return Ok( new JsonResult<LoginResult>(result));
        return BadRequest();
    }
    
    [HttpGet]
    [Authorize]
    [Route("logout")]
    public async Task Logout()
    {
        var userId = User.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
        await _authBl.LogoutUser(Guid.Parse(userId));
    }
}