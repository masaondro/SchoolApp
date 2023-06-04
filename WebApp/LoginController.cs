using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim("ExternalId", account.ExternalId),
        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}