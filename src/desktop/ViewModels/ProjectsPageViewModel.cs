using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Transleet.Desktop.Models;
using Transleet.Desktop.Services;
using Transleet.Desktop.Views;

namespace Transleet.Desktop.ViewModels
{
    [ObservableObject]
    public partial class ProjectsPageViewModel
    {
        [ObservableProperty] private ObservableCollection<Project> _projects;
        [ObservableProperty] private int _selectedIndex;
        private readonly IProjectService _projectService;
        private readonly ProjectDetialsPage _projectDetialsPage;

        public ProjectsPageViewModel(IProjectService projectService)
        {
            _projectService = projectService;
            FetchProjectsCommand = new(FetchProjectsAsync);
            NavigateToDetailsPageCommand = new(NavigateToDetailsPageAsync);
        }

        public AsyncRelayCommand FetchProjectsCommand { get; }

        public AsyncRelayCommand<ItemClickEventArgs?> NavigateToDetailsPageCommand { get; }

        private async Task FetchProjectsAsync()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            Projects = new ObservableCollection<Project>(projects);
        }

        private async Task NavigateToDetailsPageAsync(ItemClickEventArgs? args)
        {
        }
    }
}
