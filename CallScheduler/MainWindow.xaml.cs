using CallScheduler.Global;
using CallScheduler.Model;
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
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainModel _MainModel { get; set; }

        public MainWindow()
        {
            _MainModel = new MainModel();
            DataContext = _MainModel;

            InitializeComponent();

            _MainModel.SourceFilePath = Directory.GetCurrentDirectory() + @"\Data.xml";
            _MainModel.Model = new ObservableCollection<DataModel>(DataXML.XmlLoad(_MainModel.SourceFilePath));
        }
    }
}
