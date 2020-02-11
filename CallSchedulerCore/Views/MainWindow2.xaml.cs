using CallSchedulerCore.ViewModel;
using System.Windows;

namespace CallSchedulerCore.Views
{
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            DataContext = new MainViewModel2();
            InitializeComponent();
        }
    }
}
