using InstallTools.Ser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace InstallTools.view
{
    /// <summary>
    /// Installpage.xaml 的交互逻辑
    /// </summary>
    public partial class Installpage : Window
    {
        Dictionary<String, FileInfo[]> dicKV = new Dictionary<String, FileInfo[]>();
        public static String sqlpath = "D:\\MapVision3dServer_V1.01";
        private Installpage inf;
        public static Boolean R = true;
        public Installpage()
        {
            InitializeComponent();
            inf = this;
            R = true;
            Filepath();

            // IsTypeEnable = false;


        }
        public void Filepath()
        {
            DirectoryInfo rootpath;
            FileInfo[] files;
            //DirectoryInfo[] filesdics;
            string path = Directory.GetCurrentDirectory() + "\\ins";
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dics = root.GetDirectories();

            foreach (DirectoryInfo item in dics)
            {
                rootpath = new DirectoryInfo(item.FullName);
                files = rootpath.GetFiles();
                dicKV.Add(item.Name, files);
                if (item.Name == "Map3d")
                {
                    foreach (var ky in rootpath.GetDirectories())
                    {

                        files = new DirectoryInfo(ky.FullName).GetFiles();
                        dicKV.Add(ky.Name, files);
                    }
                }
            }

        }

        private void JAVABTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["Java"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void NODEBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["Node"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void REDESBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["Redis"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void VCBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["VC++"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void PGSLQBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["PostgreSQL"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void PGISBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(dicKV["PostGIS"][0].FullName);
            }
            catch (Exception)
            {
                return;
            }
        }

        public Boolean IsTypeEnable { get; set; }

        private void Map3dBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Map3dBTN.Visibility = Visibility.Hidden;

                this.Dispatcher.Invoke(new Action(() =>
                {
                    Mbar.Visibility = Visibility.Visible;
                    Map3dBTN.IsEnabled = false;
                    Map3dBTN.Content = "请稍后...";
                }));

                Task<Boolean> t = GetZ();
                Task.Run(async () =>
                {
                    bool msg = await t;
                  
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            if (msg)
                            {
                                Map3dBTN.Content = "打包完成";
                            }
                            else
                            {
                                Map3dBTN.Content = "打包失败";
                            }
                            Mbar.Visibility = Visibility.Hidden;
                            Map3dBTN.IsEnabled = true;
                        }));

                  
                });


                // new fileOper().UnZipFile(dicKV["Map3d"][0].FullName, sqlpath);

            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<Boolean> GetZ()
        {
            return await Task.Run(() =>
            {

                try
                {
                    fileOper.UnZip(dicKV["Map3d"][0].FullName, sqlpath);
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("D盘目录异常！");
                    return false;
                }

            });
        }
      

        public async void GetMain()
        {
            Dictionary<String, Int32> pth;
            String p = "D:\\Program Files";

            BrushConverter conv = new BrushConverter();
            Brush bred = conv.ConvertFromInvariantString("#CCDC4F1A") as Brush;
            Brush bgreen = conv.ConvertFromInvariantString("#CC00FF8B") as Brush;

            await Task.Run(() =>
            {

                while (R)
                {
                    try
                    {
                        DirectoryInfo root = new DirectoryInfo(p);
                        DirectoryInfo[] dics = root.GetDirectories();
                        pth = new Dictionary<string, int>();
                        pth.Add("Java", 0);
                        pth.Add("nodejs", 0);
                        pth.Add("Redis", 0);
                        pth.Add("VC++", 0);
                        pth.Add("PostgreSQL", 0);
                        pth.Add("PostgreGIS", 0);

                        inf.Dispatcher.Invoke(new Action(() =>
                        {
                            foreach (var item in dics)
                            {
                                switch (item.Name)
                                {
                                    case "Java":
                                        pth["Java"] = 1;
                                        inf.JAVABTN.Background = (System.Windows.Media.Brush)bgreen;
                                        inf.JAVABTN.Content = "Java安装（正常）";
                                        break;
                                    case "nodejs":
                                        pth["nodejs"] = 1;
                                        inf.NODEBTN.Background = (System.Windows.Media.Brush)bgreen;
                                        inf.NODEBTN.Content = "Node安装（正常）";
                                        break;
                                    case "Redis":
                                        pth["Redis"] = 1;
                                        inf.REDESBTN.Background = (System.Windows.Media.Brush)bgreen;
                                        inf.REDESBTN.Content = "Redis安装（正常）";
                                        break;
                                    case "PostgreSQL":
                                        pth["PostgreSQL"] = 1;
                                        inf.PGSLQBTN.Background = (System.Windows.Media.Brush)bgreen;
                                        inf.PGSLQBTN.Content = "PostgreSQL安装（正常）";
                                        DirectoryInfo directory = new DirectoryInfo(p + "\\" + item.Name + "\\11\\pgAdmin III");
                                        if (directory.Exists)
                                        {
                                            pth["PostgreGIS"] = 1;
                                            inf.PGISBTN.Background = (System.Windows.Media.Brush)bgreen;
                                            inf.PGISBTN.Content = "PostgreGIS安装（正常）";
                                        }
                                        else
                                            pth["PostgreGIS"] = 0;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            foreach (var key in pth.Keys)
                            {
                                switch (key)
                                {
                                    case "Java":
                                        if (pth[key] == 0)
                                        {
                                            inf.JAVABTN.Background = (System.Windows.Media.Brush)bred;
                                            inf.JAVABTN.Content = "Java安装（未安装）";
                                        };
                                        break;
                                    case "nodejs":
                                        if (pth[key] == 0)
                                        {
                                            inf.NODEBTN.Background = (System.Windows.Media.Brush)bred;
                                            inf.NODEBTN.Content = "Node安装（未安装）";
                                        }
                                        break;
                                    case "Redis":
                                        if (pth[key] == 0)
                                        {
                                            inf.REDESBTN.Background = (System.Windows.Media.Brush)bred;
                                            inf.REDESBTN.Content = "Redis安装（未安装）";
                                        }
                                        break;
                                    case "PostgreSQL":
                                        if (pth[key] == 0)
                                        {
                                            inf.PGSLQBTN.Background = (System.Windows.Media.Brush)bred;
                                            inf.PGSLQBTN.Content = "PostgreSQL安装（未安装）";
                                        }
                                        break;
                                    case "PostgreGIS":
                                        if (pth[key] == 0)
                                        {
                                            inf.PGISBTN.Background = (System.Windows.Media.Brush)bred;
                                            inf.PGISBTN.Content = "PostgreGIS安装（未安装）";
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (GetProcuct())
                            {
                                inf.VCBTN.Background = (System.Windows.Media.Brush)bgreen;
                                inf.VCBTN.Content = "VC++安装（正常）";
                            }
                            else
                            {
                                inf.VCBTN.Background = (System.Windows.Media.Brush)bred;
                                inf.VCBTN.Content = "VC++安装（未安装）";
                            }
                        }));
                        System.Threading.Thread.Sleep(5000);


                        //Action action = new Action(() => {
                        //    System.Threading.Thread.Sleep(3000);
                        //    this.Dispatcher.BeginInvoke(new Action(() =>
                        //    {
                        //    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                        //});
                        //action.BeginInvoke(null, null);
                    }
                    catch (Exception )
                    {

                    }
                }

            });
        }


        public static Boolean GetProcuct()
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey rkOpen = key.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\VisualStudio\\14.0\\VC\\Runtimes\\x64", false);
            //if (rkOpen.GetValue("Version") != null)return true; else return false;
            return rkOpen.GetValue("Version") != null ? true : false;
        }

        private void CONFIGBTN_Click(object sender, RoutedEventArgs e)
        {
            //R = false;

            configToolspage configToolspage = new configToolspage();
            Application.Current.MainWindow = configToolspage;
            this.Close();
            configToolspage.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetMain();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            //Main.Show();
        }
    }
}
