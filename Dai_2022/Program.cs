using Infrastructure;
using Infrastructure.Configuration_DB;
using Infrastructure.New;
using Infrastructure.Persistence.DbContext;
using Infrastructure.UserQueueManager;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure data protection
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DataProtectionKeys"))
            .ProtectKeysWithCertificate("5064FA301F93C975BE25ABFBAEC90941F9136AA1");

 //--------------------------------------------------//
        builder.Services.AddControllersWithViews();

    
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 4;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<DbContext>()
        .AddDefaultTokenProviders();
        //---------------------------------------------------//
       
        builder.Services.AddDB_Services(builder.Configuration);

       //-----------------------------------------------------------//
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/Login/Login";
            options.AccessDeniedPath = "/Product/AccessDenied";
            options.SlidingExpiration = true;

        });
       //-------------------------------------------------------------------------//
        builder.Services.AddDistributedMemoryCache();
        //-----------------------------------------------------------------//
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            
        });
        
        builder.Services.AddEndpointsApiExplorer();
        //-------------------------------------------------------//
        var app = builder.Build();

     
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                await RoleSeeder.SeedRolesAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
       
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                await CreateAdmin.RegisterAdminAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
       
       

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        app.UseSession();
        //app.UseMiddleware<ExceptionMiddleware>();
        //app.UseMiddleware<SaveLastVisitedX>();
        //app.UseMiddleware<PageSessionTimeoutMiddleware>();
    
         app.UseMiddleware<FileName>();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            await next();
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
