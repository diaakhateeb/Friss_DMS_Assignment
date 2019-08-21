using DataModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrissDMS.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CustomLogger.CustomLogger _logger;
        public AuthenticationController(UserManager<User> userManager)
        {
            _userManager = userManager;
            _logger = new CustomLogger.CustomLogger("AuthenticationWebApi-logs.txt");
        }

        /// <summary>
        /// Logins to the DMS system.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns>Returns HttpStatus of login attempt.</returns>
        [HttpGet("[action]")]
        public async Task<ActionResult> Login(string username, string password)
        {
            try
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Login attempt starts.",
                    "AuthenticationController_Login", null, HttpStatusCode.Created);
                var user = await _userManager.FindByNameAsync(username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                    return BadRequest(new { message = "Wrong Credential." });

                var roles = await _userManager.GetRolesAsync(user);

                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id),
                    new Claim(new IdentityOptions().ClaimsIdentity.RoleClaimType, roles.FirstOrDefault()),
                    new Claim("Username", username)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "End of Login attempt.", "AuthenticationController_Login",
                    null, HttpStatusCode.OK);
                return Ok(new { username, fullName = user.FullName, role = roles.FirstOrDefault() });
            }
            catch (NullReferenceException nullRefExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, nullRefExp.Message, nullRefExp.Source,
                    null, HttpStatusCode.BadRequest);
                return BadRequest(new { message = nullRefExp });
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, exp.Source,
                    null, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody]object data)
        {
            try
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Register attempt starts.",
                    "AuthenticationController_Register", null, HttpStatusCode.Created);
                var userData = (JObject)JsonConvert.DeserializeObject(Convert.ToString(data));
                var appUserModel = new ApplicationUserModel
                {
                    FullName = userData.First.First.SelectToken("name").Value<string>(),
                    Email = userData.First.First.SelectToken("email").Value<string>(),
                    Username = userData.First.First.SelectToken("username").Value<string>(),
                    Password = userData.First.First.SelectToken("password").Value<string>()
                };

                var newUser = new User
                {
                    FullName = appUserModel.FullName,
                    Email = appUserModel.Email,
                    UserName = appUserModel.Username
                };
                var result = await _userManager.CreateAsync(newUser, appUserModel.Password);
                if (!result.Succeeded) return BadRequest(new { messsage = "Failed User Creation." });

                await _userManager.AddToRoleAsync(newUser, userData.First.First.SelectToken("role").Value<string>());
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "End of Register attempt.", "AuthenticationController_Register",
                    null, HttpStatusCode.OK);

                return Ok(new { message = "User created Ok.", newUser });
            }
            catch (NullReferenceException nullRefExp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, nullRefExp.Message, nullRefExp.Source,
                    null, HttpStatusCode.BadRequest);
                return BadRequest(new { message = nullRefExp.Message });
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, exp.Source,
                    null, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }

        }

        [HttpGet("[action]")]
        public async Task<ActionResult> LogoutAsync()
        {
            try
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Logout attempt starts.", "AuthenticationController_Logout",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                User.Identities.ToList().RemoveAll(x => x.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("auth_cookie");
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "End of Logout attempt.", "AuthenticationController_Logout",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);
                return Ok();
            }
            catch (Exception exp)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpGet("action")]
        public ActionResult<int> AccessDenied()
        {
            return (int)HttpStatusCode.Forbidden;
        }
    }
}