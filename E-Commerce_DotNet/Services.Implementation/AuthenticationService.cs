using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared.IdentityDtos;

namespace Services.Implementation
{
    public class AuthenticationService(UserManager<User> userManager, IOptions<JwtOptions> options, IMapper mapper) : IAuthenticationService
    {
        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(user => user.Address).FirstOrDefaultAsync(user => user.Email == email);
            if (user is null)
                throw new Exception($"User With Email {email} is not found!!");

            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                throw new Exception($"User With Email {email} is not found!!");

            return new UserResultDto(
                DisplayName: user.DisplayName,
                Email: user.Email,
                Token: null
            );
        }

        public async Task<bool> IsEmailExist(string email) => await userManager.FindByEmailAsync(email) != null;

        public async Task<AddressDto> UpdateAddressAsync(string email, AddressDto addressDto)
        {
            var user = await userManager.Users.Include(user => user.Address).FirstOrDefaultAsync(user => user.Email == email);
            if (user is null)
                throw new Exception($"User With Email {email} is not found!!");

            Address newUserAddress = mapper.Map<Address>(addressDto);

            user.Address = newUserAddress;

            await userManager.UpdateAsync(user);

            return addressDto;
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new Exception($"User with email {loginDto.Email} is not found");

            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!isPasswordCorrect)
                throw new Exception($"Password is not correct");

            var token = await CreateTokenAsync(user);

            return new UserResultDto(DisplayName: user.DisplayName, Email: loginDto.Email, Token: token);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            User? user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            
            var userCreationResult = await userManager.CreateAsync(user, registerDto.Password);
            if (!userCreationResult.Succeeded)
            {
                var errors = userCreationResult.Errors.Select(e => e.Description).ToList();
                throw new Exception($"Error Creating a new user: {errors}");
            }

            var token = await CreateTokenAsync(user);

            return new UserResultDto(
                DisplayName: user.DisplayName,
                Email: registerDto.Email,
                Token: token
            );
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            JwtOptions jwtOptions = options.Value;
            // Token Header:
            // Token Payload:
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            
            // Token Signature:
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));
            var credientials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Assemble the Token:
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credientials,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpireDate),
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issuer
            );
            
            // Return the token:
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
