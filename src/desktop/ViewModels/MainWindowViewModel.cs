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
        private readonly ProjectsPageViewModel _projectsPageViewModel;

        

        public MainWindowViewModel(ProjectsPageViewModel projectsPageViewModel,NavigationManager navigationManager)
        {
            _projectsPageViewModel = projectsPageViewModel;
            NavigateToPageCommand = new (NavigateToPageAsync);
            NavigationManager = navigationManager;
        }

        public NavigationManager NavigationManager { get; set; }
        public AsyncRelayCommand<NavigationViewItemInvokedEventArgs?> NavigateToPageCommand { get; }


        private async Task NavigateToPageAsync(NavigationViewItemInvokedEventArgs? args)
        {
            if (args.IsSettingsInvoked)
            {
                await NavigationManager.NavigateToAsync(typeof(SettingsPage));
            }
            else
            {
                var item = args.InvokedItemContainer as NavigationViewItem;

                var type = (item.Tag as TypeGetter).Type;
                if (type==typeof(ProjectsPage))
                {
                    await NavigationManager.NavigateToAsync(type, _projectsPageViewModel);
                }
                else
                {
                    await NavigationManager.NavigateToAsync(type);
                }
                
            }
        }
    }
}
