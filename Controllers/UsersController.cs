using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryItems.Data;
using InventoryItems.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InventoryItems.Helper;


namespace InventoryItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUsers()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            var users = await _context.Users.ToListAsync();
            return ApiResponse.SuccessResponse(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetUser(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return ApiResponse.SuccessResponse(user);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutUser(int id, User user)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
