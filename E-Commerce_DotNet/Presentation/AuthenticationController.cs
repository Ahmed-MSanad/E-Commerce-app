using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.IdentityDtos;

namespace Presentation
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Login([FromBody] LoginDto loginDto) 
            => Ok(await serviceManager.AuthenticationService.LoginAsync(loginDto));
        [HttpPost]
        public async Task<ActionResult<RegisterDto>> Register([FromBody] RegisterDto registerDto)
            => Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDto));
        [HttpGet]
        public async Task<ActionResult<bool>> IsEmailExist(string email) => Ok(await serviceManager.AuthenticationService.IsEmailExist(email));
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentLoggedInUser()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            return Ok(await serviceManager.AuthenticationService.GetUserByEmailAsync(email));
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            return Ok (await serviceManager.AuthenticationService.GetUserAddressAsync(email));
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress([FromBody] AddressDto addressDto)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            return Ok(await serviceManager.AuthenticationService.UpdateAddressAsync(email, addressDto));
        }
    }
}
