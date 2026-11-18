using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using StorageApi.Data;

namespace StorageApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("StorageApiContext") ?? throw new System.InvalidOperationException("Connection string 'StorageApiContext' not found.");

            builder.Services.AddDbContext<StorageApiContext>(options =>
            {
                options.UseSqlServer(connectionString);

                if (builder.Environment.IsDevelopment())
                {
                    options.UseSeeding((context, _) =>
                    {
                        DbInitializer.Seed(context);
                    })
                           .UseAsyncSeeding((context, _, ct) => DbInitializer.SeedAsync(context, ct));
                }
            });
 
            builder.Services.AddControllers();
           
            builder.Services.AddOpenApi();

            var app = builder.Build();

          //  DatabaseSeeder.Seed(app.Services);

            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<StorageApiContext>();
                dbContext.Database.Migrate();

                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
