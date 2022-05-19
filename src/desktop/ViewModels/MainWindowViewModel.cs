using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Transleet.Desktop.Views;

namespace Transleet.Desktop.ViewModels
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        [ObservableProperty] private NavigationManager _navigationManager;

        public MainWindowViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            NavigateToPageCommand = new (NavigateToPageAsync);
        }

        public AsyncRelayCommand<NavigationViewSelectionChangedEventArgs?> NavigateToPageCommand { get; }

        private async Task NavigateToPageAsync(NavigationViewSelectionChangedEventArgs? args)
        {
            if (args.IsSettingsSelected)
            {
                await NavigationManager.NavigateToAsync("SettingsPage");
                return;
            }
            var item = args.SelectedItem as NavigationViewItem;
            await NavigationManager.NavigateToAsync((item.Tag as string)!);
        }
    }
}
