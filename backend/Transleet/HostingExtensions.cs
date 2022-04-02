using Microsoft.AspNetCore.Identity;
using Serilog;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Transleet.Controllers;
using Transleet.Grains;
using Transleet.Hubs;
using Transleet.Models;

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
            .AddApplicationPart(typeof(ProjectController).Assembly);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDataProtection();
        builder.Services.AddHttpClient();
        builder.Services.AddSignalR();

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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ProjectHub>("/project");
        });
        return app;
    }
}
