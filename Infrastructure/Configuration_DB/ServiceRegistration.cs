using Application;
using Application.BookingService;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbContext = Infrastructure.Persistence.DbContext.DbContext;


namespace Infrastructure.Configuration_DB
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDB_Services(this IServiceCollection collection, IConfiguration configuration)
        {
            IServiceCollection serviceCollection = collection.AddDbContext<DbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityUserDB") ,  b => b.MigrationsAssembly("Dai"));
            });
           
            collection.AddScoped<IProductService, ProductService>();
            collection.AddScoped<IProductRepository, ProductRepository_007>();

            collection.AddScoped<IBookingService, BookingService>();
            collection.AddScoped<IBookingRepository, BookingRepository>();
       
         
            return collection;
        }
    }
}
