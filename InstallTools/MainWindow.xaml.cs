using InstallTools.view;
using InstallTools.view.map;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstallTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Main;
        public MainWindow()
        {
            InitializeComponent();
            Main = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Installpage installpage = new Installpage();
            Application.Current.MainWindow = installpage;
            this.Close();
            installpage.Show();

            //this.Hide();
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void progtool_btn_Click(object sender, RoutedEventArgs e)
        {
            FileDatasurehous fileData = new FileDatasurehous();
            Application.Current.MainWindow = fileData;
            this.Close();
            fileData.Show();
        }
    }
}
