using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {
            if (loginUserDto is null)
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "User not Found!" };
            } 
            ApplicationUser? userFromDb =  await _context.applicationUsers
                                            .FirstOrDefaultAsync(au => au.Email.ToLower() == loginUserDto.Email.ToLower());
            if (userFromDb is null )
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "Check Your Email! Try Again" };
            }

            if (!BCrypt.Net.BCrypt.Verify(loginUserDto.Password, userFromDb.Password))
            {
                return new LoginResponseDto() { IsLoggedIn = false, Message = "incorrect Password!!!" };
            }
            return new LoginResponseDto() 
            {
                IsLoggedIn = true,
                Message = "Login Successfully!", 
                Token = GenerateToken(userFromDb)
            };
        }

        private string GenerateToken(ApplicationUser user)
        {
            SymmetricSecurityKey symmetricSecurityKe = new(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));

            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKe,SecurityAlgorithms.HmacSha256);

            Claim[] claims = 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email )
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims : claims,
                signingCredentials : credentials,
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires : DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTE"]))
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string jwtToken = handler.WriteToken(jwtSecurityToken);

            return jwtToken;
        }

        public async Task<RegisterReponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            if(registerUserDto == null)
            {
                return new RegisterReponseDto{IsRegistered = false, Message = "RegisterUserDto Is Null!"};
            }
            var userFormDb = await _context.applicationUsers.FirstOrDefaultAsync(au => au.Email.ToLower() == registerUserDto.Email.ToLower());
            if(userFormDb is not null)
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

            return new RegisterReponseDto { IsRegistered = true, Message = " User Has Been Registered Successfully!" };
        }
    }
}
;
