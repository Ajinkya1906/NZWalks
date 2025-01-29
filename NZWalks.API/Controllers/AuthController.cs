using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Security.AccessControl;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        //UserManager<IdentityUser> is a service provided by ASP.NET Identity, which is responsible for managing user-related tasks (e.g., creating users, deleting users, validating passwords, etc.).

        //readonly means the value of userManager can only be set once (during object creation or in the constructor).
        //This ensures that the userManager instance cannot be accidentally replaced later.
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository) 
        {
           this.userManager = userManager;
           this.tokenRepository = tokenRepository;
        }

        //AuthController -- A constructor is a special method that runs when an object of the class is created.

        //this.userManager = userManager;----> This line assigns the injected userManager to the class-level variable this.userManager.
        //Now, the userManager instance is accessible throughout the AuthController class.


        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
     
            if(identityResult.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    //Add roles to the users
                    identityResult =  await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if(identityResult.Succeeded) 
                    {
                        return Ok("User was Registered! Please login.");
                    }
                }
            }

            return Ok("Something went wrong.");
        }

        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            //class--userManager
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    //Get Roles for this User
                   var roles = await userManager.GetRolesAsync(user);

                    if(roles != null)
                    {
                      //Create Token 
                     var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                     return Ok(response);
                    }
                }
            }
            return BadRequest("Username or Password Incorrect.");
        }
        

    }
}
