//using LoadExpressApi.Application.Common.Request;
//using LoadExpressApi.Application.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace LoadExpressApi.Host.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserService _userService;
//        private readonly ILogger<UserController> _logger;

//        public UserController(IUserService userService, ILogger<UserController> logger)
//        {
//            _userService = userService;
//            _logger = logger;
//        }

//        [HttpGet("{email}")]
//        public async Task<IActionResult> Get(string email)
//        {
//            var result = await _userService.GetUserAsync(email);
//            return result.Succeeded ? Ok(result) : NotFound(result);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _userService.GetUsersAsync();
//            return result.Succeeded ? Ok(result) : BadRequest(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] UserRequest request)
//        {
//            var result = await _userService.CreateUserAsync(request);
//            return result.Succeeded ? Created("", result) : BadRequest(result);
//        }

//        [HttpPut("{email}")]
//        public async Task<IActionResult> Update(string email, [FromBody] UserRequest request)
//        {
//            var result = await _userService.UpdateUserAsync(email, request);
//            return result.Succeeded ? Ok(result) : BadRequest(result);
//        }

//        [HttpDelete("{email}")]
//        public async Task<IActionResult> Delete(string email)
//        {
//            var result = await _userService.DeleteUserAsync(email);
//            return result.Succeeded ? Ok(result) : BadRequest(result);
//        }
//    }

//}







using LoadExpressApi.Application.Common.Request;
using LoadExpressApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoadExpressApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUser/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var result = await _userService.GetUserAsync(email);
            return result.Succeeded ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            return result.Succeeded ? Created("", result) : BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateUser/{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UserRequest request)
        {
            var result = await _userService.UpdateUserAsync(email, request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("DeleteUser/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _userService.DeleteUserAsync(email);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
