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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transleet.Desktop.Pages;
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
        private readonly Page _projectsPage;
        private readonly Page _settingsPage;
        private readonly Page _profilePage;

        public MainWindow(
            IHostApplicationLifetime applicationLifetime,
            MainWindowViewModel viewModel,
            ProjectsPage projectsPage,
            SettingsPage settingsPage,
            ProfilePage profilePage)
        {
            Title = "Transleet";
            ViewModel = viewModel;
            _projectsPage = projectsPage;
            _settingsPage = settingsPage;
            _profilePage = profilePage;
            _applicationLifetime = applicationLifetime;
            InitializeComponent();
            Closed += MainWindow_Closed;
            NavigationView.SelectionChanged += NavigationView_SelectionChanged;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ViewModel.ContentPage = _settingsPage;
            }
            else
            {
                var item = args.SelectedItem as NavigationViewItem;
                switch ((string)item.Tag)
                {
                    case "ProjectsPage":
                        ViewModel.ContentPage = _projectsPage;
                        break;
                    case "ProfilePage":
                        ViewModel.ContentPage = _profilePage;
                        break;
                    default:
                        break;
                }
            }

        }

        public MainWindowViewModel ViewModel { get; set; }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            _applicationLifetime.StopApplication();
        }
    }
}
