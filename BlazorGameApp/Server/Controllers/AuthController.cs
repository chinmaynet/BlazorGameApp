using BlazorGameApp.Server.Repository;
using BlazorGameApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorGameApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserRegister request) {
            var response= await _authRepo.Register(
                new User {
                    Email = request.Email,
                    UserName = request.UserName,
                    Bananas = request.Bananas,
                    DateOfBirth = request.DateOfBirth,
                    Isconfirmed = request.IsConfirmed
                }, request.Password,request.StartUnitId);

            if (!response.Success) { 
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserLogin request)
        {
            var response = await _authRepo.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
