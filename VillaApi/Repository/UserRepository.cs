using Microsoft.EntityFrameworkCore;
using VillaApi.Data;
using VillaApi.Models;
using VillaApi.Models.Dto.LoginDto;
using VillaApi.Models.Dto.RegisterDto;
using VillaApi.Repository.IRepository;

namespace VillaApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {
            if (loginUserDto is null)
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "User not Found!" };
            } 
            var userFromDb =  await _context.applicationUsers.FirstOrDefaultAsync(au => au.Email == loginUserDto.Email);
            if (userFromDb is null )
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "Check Your Email! Try Again" };
            }

            if (BCrypt.Net.BCrypt.Verify(loginUserDto.Password, userFromDb.Password))
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "incorrect Password!!!" };
            }
            return new LoginResponseDto() 
            {
                IsLoggedIn = true,
                Message = "Login Successfully!", 
                Token = GanerateToken()
            };
        }

        private string GanerateToken()
        {

        }

        public async Task<RegisterReponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            if(registerUserDto == null)
            {
                return new RegisterReponseDto{IsRegistered = false, Message = "RegisterUserDto Is Null!"};
            }
            var userFormDb = await _context.applicationUsers.FirstOrDefaultAsync(au => au.Email == registerUserDto.Email);
            if(userFormDb is null)
            {
                return new RegisterReponseDto { IsRegistered = false, Message = "User Already Exits!" };
            }
            ApplicationUser user = new()
            {
                Email = registerUserDto.Email,
                Name = registerUserDto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword( registerUserDto.Password)
            };
            await _context.applicationUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            return new RegisterReponseDto { IsRegistered = true, Message = " User Has Been Registered Successfully!"}
        }
    }
}
