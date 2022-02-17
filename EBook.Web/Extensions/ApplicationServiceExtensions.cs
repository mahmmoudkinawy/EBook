namespace EBook.Web.Extensions;
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<DataContext>
            (options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IEmailSender, EmailSender>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddScoped<IPhotoService, PhotoService>();

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        services.Configure<StripeSettings>(config.GetSection("StripeSettings"));

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(100);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}

