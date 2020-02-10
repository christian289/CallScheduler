using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CallScheduler
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// StartupUri를 MainWindows.xaml이 지정되어 있지만, Views 네임스페이스를 추가하여 처리
        /// View 처리하는 네임스페이스를 관리하기 위함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StartupUri = new Uri("/CallScheduler;component/Views/MainWindow.xaml", UriKind.Relative);
        }
    }
}
