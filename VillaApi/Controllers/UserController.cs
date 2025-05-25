using Microsoft.AspNetCore.Mvc;
using VillaApi.Models.Dto.LoginDto;
using VillaApi.Models.Dto.RegisterDto;
using VillaApi.Repository.IRepository;

namespace VillaApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Register(RegisterUserDto registerUserDto)
        {
           var responseDto = await _userRepository.RegisterUserAsync(registerUserDto);
        }

        public async Task Login(LoginUserDto loginUserDto)
        {
            var logiDto = await _userRepository.LoginUserAsync(loginUserDto);
        }
    }
}
