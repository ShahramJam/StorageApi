using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

namespace StorageApi.Data
{
    public class StorageApiContext(DbContextOptions<StorageApiContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
    }
}