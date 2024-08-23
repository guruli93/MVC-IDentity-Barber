using Application;
using Application.Models_DB;
using Application.BookingService;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.UserQueueManager;
using Application.CloudService;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Persistence.DbContext_;

namespace Infrastructure.Configuration_DB
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDB_Services(this IServiceCollection collection, IConfiguration configuration)
        {
            // Register DbContext
            collection.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityUserDB"), b => b.MigrationsAssembly("Dai"));
            });

            // Register application services
            collection.AddScoped<IProductService, ProductService>();
            collection.AddScoped<IProductRepository, ProductRepository_007>();
            collection.AddScoped<IBookingService, BookingService>();
            collection.AddScoped<IBookingRepository, QueryBookingRepository>();
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
