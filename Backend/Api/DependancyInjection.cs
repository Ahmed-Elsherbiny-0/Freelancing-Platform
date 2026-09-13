using Api.Helpers;
using Application.Dtos.Auth;
using Application.Dtos.Auth.AuthValidators;
using Application.Interfaces;
using Application.Mapping;
using Domain.Entities.UserEntities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Helpers.Email;
using Infrastructure.Options;
using Infrastructure.Services;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Email;
using Infrastructure.Services.SignalR;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Reflection;
using System.Text;

namespace Api
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthConfig(configuration);
            services.AddScoped<IEmailSender, EmailService>();
            services.AddFluentValidationConfig().AddMapsterConfig();
            services.AddScoped<IEmailBodyBuilder,EmailBodyBuilder>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.Configure<EmailOptions>(configuration.GetSection("MailSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySetting"));

            services.AddCors(x =>
            {
                x.AddPolicy("cors", y =>
                {
                    y.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200", "https://freelancing-platform-iota.vercel.app/");
                });
            }); 

            services.AddRedisConfig(configuration);

            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtOptions>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPresenceTracker, PresenceTracker>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IWorkerService, WorkerService>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.SaveToken = true;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                       ValidIssuer = jwtSettings?.Issuer,
                       ValidAudience = jwtSettings?.Audience
                   };
                   o.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];
                           var path = context.HttpContext.Request.Path;
                           var accessToken2 = context.Request.Cookies["access_token"];
                           if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                           {
                               context.Token = accessToken;
                           }
                           if (context.Token == null&& !string.IsNullOrEmpty(accessToken2))
                           {
                               context.Token = accessToken2;
                           }
                       return Task.CompletedTask;
                       }
                   };
               });


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });


            services.AddSignalR();
            return services;

        }

        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {

            return services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();

        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {

            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(typeof(MappingConfiguration).Assembly);
            services.AddSingleton(mappingConfig);

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;

        }
        private static IServiceCollection AddRedisConfig(this IServiceCollection services, IConfiguration configuration)
        {
           return  services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var connString = configuration.GetConnectionString("Redis") ?? throw new Exception("Can not get redis connection string");

                var configurationRedis = ConfigurationOptions.Parse(connString);

                return ConnectionMultiplexer.Connect(configurationRedis);
            }
    );
        }
    }
}
