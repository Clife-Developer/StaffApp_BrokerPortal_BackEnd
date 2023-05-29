using Microsoft.AspNetCore.Mvc;
using Redefine.Broker.Dto.Models.Security;
using Redefine.Broker.Services.Interfaces;

namespace Redefine.Broker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BrokerControllerBase<UserController>
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger) : base(logger)
        {
            _userService = userService;
        }

        [HttpPost("/api/User/login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                _logger.LogTrace($"[{nameof(Login)}]: Entering Login for User [{user.Email}]");
                var userToVerify = await _userService.Login(user);

                if (userToVerify != null)
                {
                    _logger.LogTrace($"[{nameof(Login)}]: User Logged in successfully [{user.Email}]");
                    return new ObjectResult(userToVerify);
                }
                else
                {
                    _logger.LogTrace($"[{nameof(Login)}]: User was not successfully verified [{user.Email}]");
                }

                return new NotFoundObjectResult("User verification failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in {nameof(UserController.Login)}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return BadRequest(ex);
            }
        }

        [HttpPost("/api/User/create")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                _logger.LogTrace($"[{nameof(Create)}]: Entering User Creation [{user.Email}]");
                var result = await _userService.Create(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}