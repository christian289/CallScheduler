using CallScheduler.ViewModels;
using System.Windows;

namespace CallScheduler.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
