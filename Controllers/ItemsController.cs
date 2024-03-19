using InventoryItems.Models;
using InventoryItems.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using InventoryItems.Helper;

namespace InventoryItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetItems()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            var items = await _context.Items.ToListAsync();
            return ApiResponse.SuccessResponse(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetItem(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            var item = await _context.Items.FindAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }

            return ApiResponse.SuccessResponse(item);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostItem(Item itemData)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            _context.Items.Add(itemData);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = itemData.InventoryID }, itemData);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutItem(int id, Item itemData)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            if (id != itemData.InventoryID)
            {
                return BadRequest();
            }

            _context.Entry(itemData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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
        public async Task<ActionResult<ApiResponse>> DeleteItem (int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return ApiResponse.HandleUnauthorized();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.InventoryID == id);
        }
    }
}
