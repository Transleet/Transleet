using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Transleet.Desktop
{
    [ObservableObject]
    public partial class NavigationManager
    {
        [ObservableProperty] private Page _page;
        private readonly Dictionary<string,Page> _pages;

        public NavigationManager(IEnumerable<Page> pages)
        {
            _pages = pages
                .ToDictionary(_ => _.GetType().Name, _ => _);
        }

        public Task NavigateToAsync(string pageName)
        {
            Page = _pages[pageName];
            return Task.CompletedTask;
        }
    }
}
