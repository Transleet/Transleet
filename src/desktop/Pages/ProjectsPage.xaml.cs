﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Transleet.Desktop.Models;
using Transleet.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Transleet.Desktop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectsPage : Page
    {
        public ProjectsPageViewModel ViewModel { get; set; }
        public ProjectsPage(ProjectsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            this.InitializeComponent();
            Loaded += ProjectsPage_Loaded;
        }

        private async void ProjectsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.FetchProjectsAsync();
        }
    }
}
