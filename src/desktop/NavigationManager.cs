using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Transleet.Desktop
{
    [ObservableObject]
    public partial class NavigationManager
    {
        [ObservableProperty] private Page _page;

        public Task NavigateToAsync(Type pageType, object? parameter = null)
        {
            if (parameter is null)
            {
                _page = (Page)Activator.CreateInstance(pageType);
            }
            else
            {
                _page = (Page)Activator.CreateInstance(pageType, parameter);
            }

            OnPropertyChanged(nameof(Page));
            return Task.CompletedTask;
        }
    }
}
