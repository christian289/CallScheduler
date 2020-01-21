using CallScheduler.Global;
using CallScheduler.Model;
using CallScheduler.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace CallScheduler
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
