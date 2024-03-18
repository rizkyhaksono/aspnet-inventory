using InventoryItems.Models;
using InventoryItems.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await _context.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item itemData)
        {
            _context.Items.Add(itemData);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Item), itemData);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutItem(int id, Item itemData)
        {
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
                if (!ItemExists(id)) { return NotFound(); }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem (int id)
        {
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
