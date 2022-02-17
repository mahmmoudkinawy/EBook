namespace EBook.Web.Extensions;
public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders() //For generate tokens
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultUI(); //Support for Identity UI

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = $"/Identity/Account/Login";
            options.LogoutPath = $"/Identity/Account/Logout";
            options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
        });

        services.AddAuthentication().AddFacebook(options =>
        {
            options.AppId = config.GetSection("Facebook:AppId").Value;
            options.AppSecret = config.GetSection("Facebook:AppSecret").Value;
        });

        return services;
    }
}

