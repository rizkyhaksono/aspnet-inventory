using InventoryItems.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryItems.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
}
