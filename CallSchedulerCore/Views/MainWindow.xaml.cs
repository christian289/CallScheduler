using CallSchedulerCore.ViewModels;
using System.Windows;

namespace CallSchedulerCore.Views
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
