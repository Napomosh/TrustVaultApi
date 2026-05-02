using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustDrop.Auth.Bl;
using TrustDrop.Auth.Models;
using TrustDrop.Common.Error;
using TrustDrop.Common.Jwt;
using TrustDrop.Common.Result;
using TrustDrop.Common.Result.Auth;
using TrustDrop.User.Models;

namespace TrustDrop.Auth.Controllers;

[Route("api")]
[ApiController]
public class AuthController(IAuthBl authBl) : ControllerBase
{
    private readonly IAuthBl _authBl = authBl;
    
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] UserViewModel parameters)
    {
        var result = await _authBl.RegisterUser(parameters.Login, parameters.Password, parameters.Email ?? string.Empty);
        if (result.IsSuccess)
            return Ok();
        if (result.Code == ErrorCode.Conflict)
            return Conflict(new JsonResult<bool>(result));
        return BadRequest(new JsonResult<bool>(result));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] UserViewModel parameters)
    {
        var result = await _authBl.LoginUser(parameters.Login, parameters.Password);
        if (!result.IsSuccess || result.Value is null) 
            return BadRequest(new JsonResult<LoginResult>(result));
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromDays(14)
        };

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, cookieOptions);
        return Ok(new 
        {
            value = new { token = result.Value.JwtToken },
        });
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized();

        var result = await _authBl.RefreshLoginAccess(refreshToken);
        if (!result.IsSuccess || result.Value is null) 
        {
            var deleteCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Delete("refreshToken", deleteCookieOptions);
            return Unauthorized(new JsonResult<LoginResult>(result));
        }
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromDays(14)
        };

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, cookieOptions);
        return Ok(new 
        {
            value = new { token = result.Value.JwtToken },
        });
    }

    [HttpGet]
    [Authorize]
    [Route("me")]
    public async Task<ActionResult> Me()
    {
        return Ok(new UserAuthInfo
        {
            Id = new Guid(User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value),
            Name = User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value,
        });
    }

    [HttpGet]
    [Authorize]
    [Route("logout")]
    public async Task<ActionResult> Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        };
        Response.Cookies.Delete("refreshToken", cookieOptions);

        var userId = User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        await _authBl.LogoutUser(Guid.Parse(userId));

        return Ok();
    }
}