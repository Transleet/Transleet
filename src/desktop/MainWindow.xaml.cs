using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transleet.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Transleet.Desktop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
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
