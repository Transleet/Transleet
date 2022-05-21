using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Transleet.Desktop.ViewModels;

namespace Transleet.Desktop
{
    public sealed partial class MainWindow : Window
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        
        public MainWindow(
            IHostApplicationLifetime applicationLifetime,
            MainWindowViewModel viewModel)
        {
            Title = "Transleet";
            ViewModel = viewModel;
            _applicationLifetime = applicationLifetime;
            Closed += MainWindow_Closed;
            InitializeComponent();
        }

        public MainWindowViewModel ViewModel { get; set; }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            _applicationLifetime.StopApplication();
        }
    }
}
