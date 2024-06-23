using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ex3.Data.Model;
using Ex3.Data.DTO.In;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ex3.Data.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<User> userManager) : ControllerBase
{
    private static string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "GSI Challenge Authenticator",
            audience: "www.gsichanllengeapi.com",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: cred
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginDto model)
    {
        var user = await userManager.FindByNameAsync(model.UserName);
        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new ("GivenName", user.FirstName), // Add GivenName claim
                new("Surname", user.LastName), // Add Surname claim
                new("Email", user.Email), // Add Email claim
                // Add Role claim
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country", user.Country) // Add Country claim
        };

            // Get the user's roles
            var roles = await userManager.GetRolesAsync(user);

            // Add each role as a claim
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            return Ok(new { Token = GenerateJwtToken(claims) });
        }

        return Unauthorized();
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto model)
    {
        //TODO: Check if user already exists
        var user = new User { 
            UserName = model.UserName,
            Country = model.CountryCode,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        await userManager.AddToRoleAsync(user, "User");
        
        
        
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new ("GivenName", user.FirstName), // Add GivenName claim
                new("Surname", user.LastName), // Add Surname claim
                new("Email", user.Email), // Add Email claim
                // Add Role claim
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country", user.Country) // Add Country claim
        };


        return CreatedAtAction("Register", new { Token = GenerateJwtToken(claims) });
    }

    [HttpGet]
    public bool IsLogged()
    {
        return User.Identity != null && User.Identity.IsAuthenticated;
    }
}