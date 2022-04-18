using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Transleet.Controllers;
using Transleet.Hubs;
using Transleet.Models;
using Microsoft.IdentityModel.Tokens;
using Transleet.Stores;

namespace Transleet;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddApplicationPart(typeof(ProjectsController).Assembly);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDataProtection();
        builder.Services.AddHttpClient();
        builder.Services.AddSignalR();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            options.User.RequireUniqueEmail = false;

        });

        builder.Services
            .Configure<GithubOAuthOptions>(builder.Configuration.GetSection("Authentication:GitHub"));

        builder.Services.Configure<JwtOptions>(options =>
        {
            options.Issuer = builder.Configuration["Authentication:JwtBearer:Issuer"];
            options.Audience = builder.Configuration["Authentication:JwtBearer:Audience"];
            options.Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtBearer:Key"]));
        });

        builder.Services
            .AddTransient<IRoleClaimStore<Role>, OrleansRoleStore>()
            .AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>()
            .AddIdentity<User, Role>()
            .AddRoleStore<OrleansRoleStore>()   
            .AddUserStore<OrleansUserStore>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Authentication:JwtBearer:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Authentication:JwtBearer:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtBearer:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/api/hubs")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DevelopmentCorsPolicy", corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        builder.Services.AddHostedService<SearchDataUpdateService>();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("DevelopmentCorsPolicy");
        }
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ProjectsHub>("/api/hubs/projects");
            endpoints.MapHub<ComponentsHub>("/api/hubs/components");
            endpoints.MapHub<TranslationsHub>("/api/hubs/translations");
            endpoints.MapHub<EntriesHub>("/api/hubs/entries");
            endpoints.MapHub<UsersHub>("/api/hubs/users");
            endpoints.MapControllers();
        });
        return app;
    }
}
