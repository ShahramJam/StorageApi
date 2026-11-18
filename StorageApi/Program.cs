using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StorageApi.Data;

namespace StorageApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("StorageContext") ?? throw new InvalidOperationException("Connection string 'StorageContext' not found.");

            builder.Services.AddDbContext<StorageContext>(options =>
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
                var dbContext = scope.ServiceProvider.GetRequiredService<StorageContext>();
                dbContext.Database.Migrate();

                app.MapOpenApi();
            }

            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
