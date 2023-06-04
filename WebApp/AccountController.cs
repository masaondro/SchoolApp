using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Account>> Get()
        {
           
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var account = await _accountService.LoadOrCreateAsync(userId);

                if (account == null)
                {
                    return NotFound();
                }

                return account;
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<Account> GetByInternalId([FromRoute] int id)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var account = _accountService.GetFromCache(id);

                if (account == null)
                {
                    return NotFound();
                }

                return account;
            }
            else
            {
                return Unauthorized();
            }
            
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task<IActionResult> UpdateAccount()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Task<ActionResult<Account>> result = Get();
                ActionResult<Account> actionResult = await result;
                Account account = actionResult.Value;
                if (account != null)
                {
                    account.Counter++;
                }
                else
                {
                    return NotFound();
                }

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}