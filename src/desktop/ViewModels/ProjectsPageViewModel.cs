using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Transleet.Desktop.Models;
using Transleet.Desktop.Services;

namespace Transleet.Desktop.ViewModels
{
    [ObservableObject]
    public partial class ProjectsPageViewModel
    {
        [ObservableProperty] private ObservableCollection<Project> _projects;

        private readonly IProjectService _projectService;

        public ProjectsPageViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task FetchProjectsAsync()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            Projects = new ObservableCollection<Project>(projects);
        }
    }
}
