using System;
using System.IO.IsolatedStorage;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Refit;
using Transleet.Desktop;
using Transleet.Desktop.Pages;
using Transleet.Desktop.Services;
using Transleet.Desktop.ViewModels;
using WinRT;

namespace App1
{
    internal class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context,config) =>
                {
                    config.AddJsonFile("./appsettings.json");

                })
                .ConfigureServices(services =>
            {
                services.AddTransient<ProjectsPage>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<ProfilePage>();
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<ProjectsPageViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddRefitClient<IProjectService>().ConfigureHttpClient(options =>
                {
                    options.BaseAddress = new Uri("https://localhost:7999/api");
                });
                services.AddHostedService<AppStartupService>();
            }).Build().Run();
        }
    }
}
