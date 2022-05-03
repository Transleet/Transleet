using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Transleet.Hubs;
using Transleet.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDbGenericRepository;
using Transleet.Repositories;
using Transleet.Services;

namespace Transleet;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
            options.ModelBinderProviders.Insert(0,new ObjectIdModelBinderProvider());
        })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonObjectIdConverter());
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerGeneratorOptions.Servers.Add(new OpenApiServer { Url = builder.Configuration["BackendServerUrl"] });
            options.SchemaFilter<ObjectIdSchemaFilter>();
        });
        builder.Services.AddDataProtection();
        builder.Services.AddHttpClient();
        builder.Services.AddSignalR().AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.Converters.Add(new JsonObjectIdConverter());
            options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

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
        var mongoDbContext = new MongoDbContext(builder.Configuration["Database:ConnectionString"], builder.Configuration["Database:Name"]);
        builder.Services.AddIdentity<User, Role>()
            .AddMongoDbStores<User, Role, ObjectId>(mongoDbContext);

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
        
        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton<IComponentRepository, ComponentRepository>();
        builder.Services.AddSingleton<IProjectService, ProjectService>();
        builder.Services.AddSingleton<IComponentService, ComponentService>();
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
            endpoints.MapControllers();
        });
        return app;
    }
}
