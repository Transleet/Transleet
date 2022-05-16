using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Transleet.Desktop.ViewModels
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        [ObservableProperty] private Page _contentPage;
    }
}
