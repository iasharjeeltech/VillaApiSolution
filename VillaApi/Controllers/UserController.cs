using Microsoft.AspNetCore.Mvc;
using VillaApi.Models.Dto.LoginDto;
using VillaApi.Models.Dto.RegisterDto;
using VillaApi.Repository.IRepository;

namespace VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("Register")]
        public async Task<RegisterReponseDto> Register(RegisterUserDto registerUserDto)
        {
           var responseDto = await _userRepository.RegisterUserAsync(registerUserDto);
            return responseDto;
        }
        [HttpPost("Login")]
        public async Task<LoginResponseDto> Login(LoginUserDto loginUserDto)
        {
            var loginDto = await _userRepository.LoginUserAsync(loginUserDto);
            return loginDto;
        }
    }
}
