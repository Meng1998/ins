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

namespace InstallTools.view.map
{
    /// <summary>
    /// NestChrome.xaml 的交互逻辑
    /// </summary>
    public partial class NestChrome : Window
    {
        public NestChrome()
        {
            InitializeComponent();
            
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.Address = "http://192.168.0.178:19901/aimapvision3d/mapdemo.html?id=cf214a5c0326408cbe05b58e92315719&token=38b56b60d7538e242518e7a32b9be88c";
        }
    }
}
