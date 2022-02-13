﻿namespace EBook.Web.Extensions;
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<DataContext>
            (options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IEmailSender, EmailSender>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPhotoService, PhotoService>();

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        return services;
    }
}
