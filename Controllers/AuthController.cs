using InventoryItems.Data;
using InventoryItems.Dto;
using InventoryItems.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventoryItems.Helper;

namespace InventoryItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly SymmetricSecurityKey _jwtSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
        private readonly string _jwtIssuer = configuration["JwtSettings:Issuer"];
        private readonly string _jwtAudience = configuration["JwtSettings:Audience"];

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLoginDto userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username && u.Password == userLogin.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            user.AccessToken = token;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var response = new
            {
                data = new
                {
                    accessToken = token,
                    createdAt = user.CreatedAt,
                    updatedAt = user.UpdateAt
                }
            };

            return Ok(response);
        }

        [HttpPost("Logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ApiResponse>> Logout()
        {
            var accessToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is missing.");
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(accessToken) as JwtSecurityToken;
            if (token == null)
            {
                return BadRequest("Invalid access token.");
            }

            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user information.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the access token provided matches the user's access token
            if (user.AccessToken != accessToken)
            {
                return BadRequest("Invalid access token for this user.");
            }

            // Remove the access token from the user
            user.AccessToken = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return ApiResponse.SuccessResponse("Logged out successfully.");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(_jwtSecretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
