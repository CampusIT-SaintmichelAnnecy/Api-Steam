using System.Windows;
using APISteam.ViewModels;
using MahApps.Metro.Controls;

namespace APISteam.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}