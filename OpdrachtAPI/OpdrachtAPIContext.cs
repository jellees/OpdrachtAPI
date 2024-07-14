using Microsoft.EntityFrameworkCore;
using OpdrachtAPI.Models;

namespace OpdrachtAPI;

public class OpdrachtAPIContext : DbContext
{
    public OpdrachtAPIContext(DbContextOptions<OpdrachtAPIContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}
