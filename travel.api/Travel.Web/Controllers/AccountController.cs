using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Travel.Web.Models;
using Travel.Web.Reposistory;



public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitofWork;
    [HttpGet]
    public IActionResult Login()
    {
        return   View();

    }

    public AccountController(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitofWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        var user = await _unitofWork.Users.ValidateUser(request.Username, request.Password); // Fix for Problem 1: Corrected method name
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()), // Fix for Problem 2: Changed 'Id' to 'UserID'
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, "User") // Fix for Problem 4: Hardcoded role as 'User' since 'Role' is not defined in User class
            };

        var secret = _configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secret)) // Fix for Problem 6: Added null check for Jwt:Secret
        {
            throw new ArgumentNullException(nameof(secret), "JWT Secret is not configured.");
        }

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
