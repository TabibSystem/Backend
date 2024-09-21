using System.Text;
using EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using TabibApp.Infrastructure.Data;
using TabibApp.Infrastructure.DependancyInjection;

public static class ServiceContainercs
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
         var connectionString = configuration.GetConnectionString("productionconnection");
        var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfigration>();
         services.Configure<JwtSettings>(configuration.GetSection("jwt"));

        services.AddSingleton(emailConfiguration);

    services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("productionconnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    services.Configure<DataProtectionTokenProviderOptions>(op =>op.TokenLifespan = TimeSpan.FromHours(2));

       services.AddTransient<IEmailSender, EmailSender>();
      

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["jwt:Issuer"],
                ValidAudience = configuration["jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });
        

        return services;
    }
  
}