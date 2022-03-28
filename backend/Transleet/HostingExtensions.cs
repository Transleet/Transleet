using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository;
using Serilog;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Transleet.Hubs;
using Transleet.Models;
using Transleet.Services;

namespace Transleet;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        var mongoDbContext = new MongoDbContext(builder.Configuration["AppDatabase:ConnectionString"],
            builder.Configuration["AppDatabase:Database"]);
        builder.Services.AddSingleton(mongoDbContext);
        builder.Services.AddSingleton(mongoDbContext.Database);
        builder.Services.AddSingleton<IService<Entry>, EntriesService>();
        builder.Services.AddSingleton<IService<Project>, ProjectsService>();
        builder.Services.AddSingleton<IService<Label>, LabelsService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDataProtection();
        builder.Services.AddHttpClient();
        builder.Services.AddSignalR();

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            })
            .AddMongoDbStores<IMongoDbContext>(mongoDbContext)
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;

            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
        });
        builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                options.UseMongoDb();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                options.SetTokenEndpointUris("/authorize/token", "/oauth/github_callback3");

                options.AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .AllowCustomFlow("github");

                options.AcceptAnonymousClients();


                options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Roles, "dataEventRecords");

                

                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();
            })
            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Authentication:JwtBearer:Issuer"];
                options.Audience = builder.Configuration["Authentication:JwtBearer:Audience"];
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
            }).AddOAuth("Github", options =>
            {
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.CallbackPath = "/external/github_callback";
                options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
                options.SaveTokens = true;
                options.Scope.Add("user");
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("myCorsPolicy", corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .WithOrigins("https://github.com","http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        


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
        }
        app.UseCors("myCorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<HelloHub>("/hello");
            endpoints.MapHub<ProjectHub>("/project");
        });
        return app;
    }
}
