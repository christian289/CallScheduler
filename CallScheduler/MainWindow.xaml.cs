using CallScheduler.ViewModel;
using System.Windows;

namespace CallScheduler
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
