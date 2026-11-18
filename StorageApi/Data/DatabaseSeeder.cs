using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Models;

namespace StorageApi
{
    //public static class DatabaseSeeder
    //{
    //    public static void Seed(IServiceProvider services)
    //    {
    //        using var scope = services.CreateScope();
    //        var context = scope.ServiceProvider.GetRequiredService<StorageContext>();
    //        context.Database.EnsureCreated();

    //        if (!context.Product.Any())
    //        {
    //            context.Product.AddRange(
    //                new Product
    //                {
    //                    Name = "Wireless Mouse",
    //                    Price = 25,
    //                    Category = "Electronics",
    //                    Shelf = "A1",
    //                    Count = 40,
    //                    Description = "Ergonomic wireless mouse"
    //                },
    //                new Product
    //                {
    //                    Name = "Mechanical Keyboard",
    //                    Price = 75,
    //                    Category = "Electronics",
    //                    Shelf = "A2",
    //                    Count = 15,
    //                    Description = "Compact mechanical keyboard"
    //                });
    //            context.SaveChanges();
    //        }
    //    }
    //}
}
