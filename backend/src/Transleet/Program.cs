using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using Serilog;
using Transleet;
using Transleet.Grains;
using Transleet.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.Configure<ClusterMembershipOptions>(options =>
        {
            options.ValidateInitialConnectivity = false;
        });
        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "dev";
        });
        siloBuilder.ConfigureApplicationParts(parts =>
        {
            parts.AddApplicationPart(typeof(LookupGrain<>).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(IdentityUser<>).Assembly).WithReferences();
        });

        var invariant = "System.Data.SqlClient";
        var connectionString = builder.Configuration["Database:ConnectionString"];
        siloBuilder.UseAdoNetClustering(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.UseAdoNetReminderService(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.AddAdoNetGrainStorage("Default", options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.AddAdoNetGrainStorage("PubSubStore", options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.AddSimpleMessageStreamProvider("SMS");
        siloBuilder.ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000);

    });

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

var app = builder.ConfigureServices()
    .ConfigurePipeline();

app.Run();
