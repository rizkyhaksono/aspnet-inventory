using InventoryItems.Models;
using InventoryItems.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace InventoryItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HandleUnauthorized();
            }

            return await _context.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HandleUnauthorized();
            }

            var item = await _context.Items.FindAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item itemData)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HandleUnauthorized();
            }

            _context.Items.Add(itemData);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = itemData.InventoryID }, itemData);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutItem(int id, Item itemData)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HandleUnauthorized();
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
        public async Task<ActionResult> DeleteItem (int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HandleUnauthorized();
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

        private ActionResult<IEnumerable<Item>> HandleUnauthorized()
        {
            return StatusCode(401, "Unauthorized access. Please provide a valid access token.");
        }
    }
}
