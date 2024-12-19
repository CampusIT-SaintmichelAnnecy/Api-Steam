using System.Windows.Controls;
using APISteam.ViewModels;

namespace APISteam.Views
{
    public partial class NewReleasesView : UserControl
    {
        public NewReleasesView()
        {
            InitializeComponent();
            DataContext = new NewReleasesViewModel();
        }
    }
}