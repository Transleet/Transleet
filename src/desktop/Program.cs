using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml.Controls;
using Refit;
using Transleet.Desktop.Services;
using Transleet.Desktop.ViewModels;
using Transleet.Desktop.Views;

namespace Transleet.Desktop
{
    internal class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ProjectsPageViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<NavigationManager>();
                    services.AddRefitClient<IProjectService>().ConfigureHttpClient(options =>
                    {
                        options.BaseAddress = new Uri("https://localhost:57999/api");
                    });
                    services.AddHostedService<AppStartupService>();
                }).Build().Run();
        }
    }
}
