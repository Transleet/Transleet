using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using WinRT;

namespace Transleet.Desktop
{
    internal class AppStartupService:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppStartupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ComWrappersSupport.InitializeComWrappers();
            Application.Start(p =>
            {
                var context =
                    new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                new App(_serviceProvider);
            });
            return Task.CompletedTask;
        }
    }
}
