using Application;
using Application.Models_DB;
using Application.BookingService;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbContext = Infrastructure.Persistence.DbContext.DbContext;
using Infrastructure.UserQueueManager;
using Application.CloudService;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;

namespace Infrastructure.Configuration_DB
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDB_Services(this IServiceCollection collection, IConfiguration configuration)
        {
            // Register DbContext
            collection.AddDbContext<DbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityUserDB"), b => b.MigrationsAssembly("Dai"));
            });

            // Register application services
            collection.AddScoped<IProductService, ProductService>();
            collection.AddScoped<IProductRepository, ProductRepository_007>();
            collection.AddScoped<IBookingService, BookingService>();
            collection.AddScoped<IBookingRepository, BookingRepository>();
            collection.AddSingleton<UserQueueManagerX>();

            //// Register Google Cloud Storage client
            //var bucketName = configuration["GoogleCloud:BucketName"];
            //var credentialsFilePath = configuration["GoogleCloud:CredentialsFilePath"];

            //collection.AddSingleton(provider =>
            //{
            //    var credential = GoogleCredential.FromFile(credentialsFilePath)
            //        .CreateScoped("https://www.googleapis.com/auth/devstorage.full_control");
            //    return StorageClient.Create(credential);
            //});

            //collection.AddScoped<IGoogleCloudStorageService>(provider =>
            //{
            //    var storageClient = provider.GetRequiredService<StorageClient>();
            //    return new GoogleCloudStorageService(storageClient, bucketName);
            //});

            return collection;
        }
    }
}
