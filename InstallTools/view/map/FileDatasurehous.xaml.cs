using InstallTools.model;
using InstallTools.Ser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
//using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace InstallTools.view.map
{
    /// <summary>
    /// FileDatasurehous.xaml 的交互逻辑
    /// </summary>
    public partial class FileDatasurehous : Window
    {
        private String sqlstr = "";
        private FileDatasurehous filemain;
        // private NpgsqlConnection conn;

        public FileDatasurehous()
        {
            InitializeComponent();
            filemain = this;
           // String cc = Convert.ToString(123456789, 16);
        }


        private string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        private void sqlConnBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (fileOper.sqlInit(this.AddTxt.Text, this.MStxt.Text, this.PasswordBox.Password))
                    {
                        this.sqlConnBtn.Content = "连接成功";
                    }
                    else
                    {
                        this.sqlConnBtn.Content = "连接失败";
                    }
                }));
            }
            catch (Exception)
            {
            }
        }

        //树数据
        public  TreeViewItem treelist ;
        private void DataFile_btn_Click(object sender, RoutedEventArgs e)
        {
            //清理树
            treelist  = new TreeViewItem();
            treeView.ItemsSource = null;
            treeView.Items.Clear();
            try
            {
                if (fileOper.FilePathname())
                {
                    //递归生成数据
                    fileOper.BindTreeView2(fileOper.filepath, treelist);
                    treeView.ItemsSource = treelist.Items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DataImport_btn_Click(object sender, RoutedEventArgs e)
        {
            //EnumService.GetDescription(AuditEnum.Auditing);

            this.Dispatcher.Invoke(new Action(() =>
            {
                if (fileOper.DataIns())
                {
                    this.DataImport_btn.Content = "导入成功";
                }
                else
                {
                    this.DataImport_btn.Content = "导入失败";
                }
            }));

        }

        private void sqlCreate_Click(object sender, RoutedEventArgs e)
        {
            String strmsg = "";

            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    String sqlstr = $"CREATE DATABASE {this.MStxt.Text}";
                    String conn = $"Server={this.AddTxt.Text};Port=5432;USER ID=postgres;Password={this.PasswordBox.Password};";
                    pgsql.GetSql(sqlstr, conn);
                    strmsg += "数据库创建成功";

                    FileInfo file = new FileInfo(Directory.GetCurrentDirectory() + "\\database\\basics.sql");
                    sqlstr = file.OpenText().ReadToEnd();
                    strmsg += "文件读取完成";

                    conn += $"Database={this.MStxt.Text}";
                    pgsql.GetSql(sqlstr, conn);
                    strmsg += "表结构创建完成";

                    this.sqlCreate_btn.Content = "创建完成";
                }));

            }
            catch (Exception ex)
            {
                strmsg += "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                          "错误提示：" + ex.Message;
                this.sqlCreate_btn.Content = "创建失败";
                Log.Debug(strmsg);
            }

        }

        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    MainWindow Main = new MainWindow();
        //    Application.Current.MainWindow = Main;
        //    Main.Show();
        //    filemain.Close();
        //}
    }
}
