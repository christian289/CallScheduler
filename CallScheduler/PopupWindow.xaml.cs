using CallScheduler.Model;
using CallScheduler.ViewModel;
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
using System.Windows.Shapes;

namespace CallScheduler
{
    public partial class PopupWindow : Window
    {
        public PopupWindow(DataModel obj)
        {
            DataContext = new PopupViewModel(obj);
            InitializeComponent();
        }
    }
}
