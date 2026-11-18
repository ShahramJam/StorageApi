using Microsoft.EntityFrameworkCore;
using StorageApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; // Adjust namespace to match your entities

namespace StorageApi.Data;

public static class DbInitializer
{
    public static void Seed(DbContext context)
    {
        if (context.Set<Product>().Any())
        {
            return; // Database already has data
        }

        context.Set<Product>().AddRange(GetInitialProducts());
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Set<Product>().AnyAsync(cancellationToken))
        {
            return; // Database already has data
        }

        await context.Set<Product>().AddRangeAsync(GetInitialProducts(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static List<Product> GetInitialProducts()
    {
        return [
         new Product
                    {
                        Name = "Wireless Mouse",
                        Price = 25,
                        Category = "Electronics",
                        Shelf = "A1",
                        Count = 40,
                        Description = "Ergonomic wireless mouse"
                    },
                    new Product
                    {
                        Name = "Mechanical Keyboard",
                        Price = 75,
                        Category = "Electronics",
                        Shelf = "A2",
                        Count = 15,
                        Description = "Compact mechanical keyboard"
                    }
    ];
    }
}
