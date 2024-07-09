using HocTiengAnh.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace HocTiengAnh.Views.Pages
{
    /// <summary>
    /// Interaction logic for AppLoggingPage.xaml
    /// </summary>
    public partial class AppLoggingPage : INavigableView<AppLoggingViewModel>
    {
        public AppLoggingViewModel ViewModel { get; }
        public AppLoggingPage(AppLoggingViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
