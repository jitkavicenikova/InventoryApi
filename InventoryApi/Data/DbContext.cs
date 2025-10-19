using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data;

/// <summary>
/// Application DbContext.
/// </summary>
public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Stock> Stocks { get; set; } = null!;
    public DbSet<StockMovement> StockMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Stock>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<StockMovement>().HasQueryFilter(sm => !sm.Stock.IsDeleted);
    }
}
