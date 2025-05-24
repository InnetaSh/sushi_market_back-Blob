//using Microsoft.AspNetCore.Mvc;
//using _26._02_sushi_market_back.Services;
//using _26._02_sushi_market_back.Models;

//namespace _26._02_sushi_market_back.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class RegisterController : Controller
//    {
//        private readonly IConfiguration _config;
//        private readonly RegisterService _regService;

//        public RegisterController(RegisterService regService, IConfiguration config)
//        {
//            _regService = regService;
//            _config = config;
//        }


//        [HttpGet]
//        public ActionResult<List<User>> GetAllUsers()
//        {

//            var users = _regService.GetAllUsers();
//            return Ok(users);

//        }

//        [HttpPost("register")]
//        public IActionResult Register([FromBody] UserRequestReg request)
//        {
//            if (request == null)
//                return BadRequest("Request is null");

//            var user = _regService.Register(request);
//            var tests = _regService.Register(request);
//            return Ok(tests);
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] UserRequestLog request)
//        {
//            if (request == null)
//                return BadRequest("Request is null");

//            var user = _regService.Login(request);

//            if (user != null)
//            {
//                return Ok("Вход в систему выполнен успешно");
//            }
//            else
//            {
//                return Unauthorized("Неверное имя пользователя или пароль");
//            }
//        }


//    }
//}



using _26._02_sushi_market_back.Models;
using _26._02_sushi_market_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace _26._02_sushi_market_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _regService;

        
        public RegisterController(RegisterService regService)
        {
            _regService = regService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _regService.GetAllUsers();
            return Ok(users);
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRequestReg request)
        {
            if (request == null)
                return BadRequest("Request is null");

            try
            {
                var user = _regService.Register(request);
                return Ok(user); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserRequestLog request)
        {
            if (request == null)
                return BadRequest("Request is null");

            var user = _regService.Login(request);

            if (user != null)
            {
                return Ok("Вход в систему выполнен успешно");
            }
            else
            {
                return Unauthorized("Неверное имя пользователя или пароль");
            }
        }
    }
}
