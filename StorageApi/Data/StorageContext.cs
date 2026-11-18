using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

public class StorageContext(DbContextOptions<StorageContext> options) : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
}
