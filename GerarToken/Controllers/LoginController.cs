using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GerarToken.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GerarToken.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginUserDto loginUserDto)
    {
        if (loginUserDto.Email != "admin123@gmail.com" || loginUserDto.Password != "Gl1234$")
        {
            return BadRequest("Login Ou senha invalidos(a)!");
        }

        var chavePrivada = _configuration.GetSection("JWT").GetValue<string>("SecretKey");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chavePrivada!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Joao"),
            new Claim(ClaimTypes.Email, "joao@email.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        //Console.WriteLine($"Seu token é {new JwtSecurityTokenHandler().WriteToken(token)}");
        
        return Ok("Seu token é: " + new JwtSecurityTokenHandler().WriteToken(token));
    }
}