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
using System.Windows.Threading;

namespace InstallTools.view
{
    /// <summary>
    /// configToolspage.xaml 的交互逻辑
    /// </summary>
    public partial class configToolspage : Window
    {
        public configToolspage conf;
        public configToolspage()
        {
            InitializeComponent();
            conf = this;
        }

        public delegate void Uphead();

        private void sqlConnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Mbar.Visibility = Visibility.Visible;
                sqlConnBtn.IsEnabled = false;
                sqlConnBtn.Content = "请稍后...";
                PassTxt.Password = PasswordBox.Password;
                MsTxt.Text = MStxt.Text;
            }));

            Task<Boolean> t = GetV();
            Task.Run(async () =>
            {
                bool msg = await t;
                this.Dispatcher.Invoke(new Action(() =>
                {

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        if (msg)
                        {
                            sqlConnBtn.Content = "创建成功";
                        }
                        else
                        {
                            sqlConnBtn.Content = "创建失败";
                        }
                        Mbar.Visibility = Visibility.Hidden;
                        sqlConnBtn.IsEnabled = true;
                    }));
                }));
            });




        }


        public async Task<Boolean> GetV()
        {
            return await Task.Run(() =>
            {
                String conn = "";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    conn = $"Server={conf.AddTxt.Text};Port=5432;DATABASE=postgres;USER ID=postgres;Password={conf.PasswordBox.Password};";
                }));

                String sqlgis = "CREATE extension postgis;";
                String sql = " create schema mapv3d authorization postgres; ";
                try
                {
                    if (!Ser.pgsql.GetSql(sqlgis, conn))
                    {
                        MessageBox.Show("Postgis未安装！");
                        return false;
                    }

                    if (!Ser.pgsql.GetSql(sql, conn))
                    {
                        return false;
                    }

                    FileInfo file = new FileInfo(Installpage.sqlpath + "\\init.sql");
                    String datasql = file.OpenText().ReadToEnd();
                    Ser.pgsql.GetSql(datasql, conn);
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("数据输入异常！");
                    return false;
                }

            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileInfo file = new FileInfo(Installpage.sqlpath + "\\启动（未注册服务测试）.bat");
            // Ciftxt.Text = file.OpenText().ReadToEnd();


        }

        private void Map3dBN_Click(object sender, RoutedEventArgs e)
        {
            //  String str = $"java -jar ./aimapvision-3d-server.jar --spring.profiles.active=prod --spring.redis.host=127.0.0.1  --spring.redis.port=6379 --spring.redis.password= --spring.datasource.druid.master.username=postgres --spring.datasource.druid.master.password={PasswordBox.Password} --spring.datasource.druid.master.url=\"jdbc: postgresql://127.0.0.1:5432/postgres?currentSchema={MStxt.Text}\" --file.upload.path=D:/MapVision3dServer_V1.01/data";
            String str = $"java -jar ./aimapvision-3d-server.jar --spring.profiles.active=prod --spring.redis.host={HostTxt.Text}  --spring.redis.port={PortTxt.Text} --spring.redis.password= --spring.datasource.druid.master.username={UserTxt.Text} --spring.datasource.druid.master.password={PassTxt.Password} --spring.datasource.druid.master.url=\"jdbc:postgresql://127.0.0.1:5432/postgres?currentSchema={MsTxt.Text}\" --file.upload.path=D:/MapVision3dServer_V1.01/data";
            //File.WriteAllText(Installpage.sqlpath + "\\启动（未注册服务测试）.bat", str, Encoding.UTF8);


            using (var fs = new FileStream(Installpage.sqlpath + "\\启动（未注册服务测试）.bat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(str);
                }
            }
            // Ciftxt.Text = new FileInfo(Installpage.sqlpath + "\\启动（未注册服务测试）.bat").OpenText().ReadToEnd();
        }

        private void Map3dN_Click(object sender, RoutedEventArgs e)
        {

            //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            //info.FileName = Installpage.sqlpath + "\\启动（未注册服务测试）.bat";
            //System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
            //proc.WaitForExit();

            GetP();
        }


        public async void GetP()
        {
            await Task.Run(() =>
            {
                Process proc = null;
                try
                {
                    //string batPath = @"D:\MapVision3dServer_V1.01\启动（未注册服务测试）.bat";
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = string.Format(@"D:\MapVision3dServer_V1.01\");
                    proc.StartInfo.FileName = "启动（未注册服务测试）.bat";
                    proc.StartInfo.Arguments = string.Format("10");//this is argument
                    proc.Start();
                    proc.WaitForExit();

                }
                catch (Exception)
                {
                }

            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow Main = new MainWindow();
            Application.Current.MainWindow = Main;
            this.Close();
            Main.Show();
        }
    }
}
