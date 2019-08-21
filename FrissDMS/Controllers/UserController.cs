using DataModel;
using DocumentRepositoryService.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FrissDMS.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IDocumentRepository<Document> _docRepo;
        private readonly CustomLogger.CustomLogger _logger;

        public UserController(UserManager<User> userManager, IDocumentRepository<Document> docRepo)
        {
            _userManager = userManager;
            _docRepo = docRepo;
            _logger = new CustomLogger.CustomLogger("UserWebApi-logs.txt");
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                _logger.Log(LogLevel.Information, "GetUsers starts.", "UserController_GetUsers",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                return Ok(_userManager.Users);
            }
            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpGet("[action]")]
        public ActionResult<object> GetUserById(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, "GetUserById starts.", "UserController_GetUserById",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);

                var user = _userManager.FindByIdAsync(id).Result;
                var role = _userManager.IsInRoleAsync(user, "Admin").Result ? "Admin" : "Member";

                _logger.Log(LogLevel.Information, "GetUserById ends.", "UserController_GetUserById",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return new { user, role };
            }
            catch (NullReferenceException nullRefExp)
            {
                _logger.Log(LogLevel.Error, nullRefExp.Message, nullRefExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = nullRefExp.Message });
            }
            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> EditUser([FromBody] object data)
        {
            try
            {
                _logger.Log(LogLevel.Information, "EditUser starts.", "UserController_EditUser",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                var userData = (JObject)JsonConvert.DeserializeObject(Convert.ToString(data));
                var user = await _userManager.FindByIdAsync(userData.SelectToken("id").Value<string>());

                user.FullName = userData.SelectToken("name").Value<string>();
                user.UserName = userData.SelectToken("username").Value<string>();
                user.Email = userData.SelectToken("email").Value<string>();

                //update password.
                if (userData.SelectToken("password") != null)
                {
                    var password = userData.SelectToken("password").Value<string>();
                    if (!string.IsNullOrEmpty(password) || !string.IsNullOrWhiteSpace(password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token,
                            userData.SelectToken("password").Value<string>());
                    }
                }

                //update role.
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, userData.SelectToken("role").Value<string>());

                _logger.Log(LogLevel.Information, "GetUserById ends.", "UserController_EditUser",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);

                return Ok();
            }
            catch (ArgumentNullException argExp)
            {
                _logger.Log(LogLevel.Error, argExp.Message, argExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = argExp.Message });
            }
            catch (NullReferenceException nullExp)
            {
                _logger.Log(LogLevel.Error, nullExp.Message, nullExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = nullExp.Message });
            }
            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }

        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, "DeleteUser starts.", "UserController_DeleteUser",
                    User.FindFirst("Username").Value, HttpStatusCode.Created);
                if (id == User.FindFirst("UserId").Value)
                    return BadRequest("You try to delete your own account!");

                var userDocsCount = _docRepo.GetAll().Count(x => x.UserId == id);
                if (userDocsCount > 0) return BadRequest("User account has documents attached.");

                var user = await _userManager.FindByIdAsync(id);
                var deleteResult = await _userManager.DeleteAsync(user);

                if (deleteResult.Succeeded)
                {
                    _logger.Log(LogLevel.Information, "DeleteUser ends.", "UserController_DeleteUser",
                    User.FindFirst("Username").Value, HttpStatusCode.OK);
                    return Ok();
                }
                _logger.Log(LogLevel.Information, "DeleteUser ends.", "UserController_DeleteUser",
                                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(deleteResult.Errors);
            }
            catch (NullReferenceException nullRefExp)
            {
                _logger.Log(LogLevel.Error, nullRefExp.Message, nullRefExp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = nullRefExp.Message });
            }

            catch (Exception exp)
            {
                _logger.Log(LogLevel.Error, exp.Message, exp.Source,
                    User.FindFirst("Username").Value, HttpStatusCode.BadRequest);
                return BadRequest(new { message = exp.Message });
            }
        }
    }
}