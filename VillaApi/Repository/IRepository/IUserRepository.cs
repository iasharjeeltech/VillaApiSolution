using VillaApi.Models.Dto.LoginDto;
using VillaApi.Models.Dto.RegisterDto;

namespace VillaApi.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<RegisterReponseDto> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
    }
}
