using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // method CreateJWTToken is designed to generate a JWT token.
        //IdentityUser user: Represents the user for whom the token is being created. It includes properties like Email, UserName, etc.
        //List<string> roles: A list of roles assigned to the user, like "Admin", "Editor", etc.
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {

            //Create Claims
            var claims = new List<Claim>();


            //Claim(type,value)
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

                foreach(var role in roles) 
                {
                  claims.Add(new Claim(ClaimTypes.Role, role));
                }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credential = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credential);
   
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}