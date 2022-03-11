using InstallTools.Ser;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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
    /// onitor.xaml 的交互逻辑
    /// </summary>
    public partial class onitor : Window
    {
        private static string path;
        private static String strblock;
        private static onitor main;
        private static Boolean ISread;
        public onitor()
        {
            InitializeComponent();
            main = this;
            new startConfig();
            KillProcess("cmd");
        }

        private void Openexe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ISread = true;
                String pathstr="";

                main.Dispatcher.Invoke(new Action(() =>
                {
                    this.Closeexe.Visibility = Visibility.Visible;
                }));


                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();


                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                {
                    //String[] str = openFileDialog.FileName.Split('/');
                    String name = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(@"\") + 1); //System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    // pathstr = openFileDialog.SafeFileName;
                    pathstr+= "\r\n";
                    pathstr +=$"cmd /k \"cd /d {System.IO.Path.GetDirectoryName(openFileDialog.FileName)}&&{openFileDialog.SafeFileName}\"";
                    path = Directory.GetCurrentDirectory() + "\\start.bat";

                    System.IO.File.WriteAllText(path, pathstr, Encoding.UTF8);
                    Main(openFileDialog.SafeFileName.Split('.')[0]);
                }


              
                   
                    //Process.Start(path);

            }
            catch (Exception ex)
            {
                Log.Debug("线程操作异常：", ex.Message);
            }
        }

        private async void Main(String JCname)
        {
            while (ISread)
            {

                await Task.Run(() =>
                {
                    Boolean FF = true;
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName == JCname)
                        {
                            FF=false;
                            break;
                        }
                    }

                    if (FF)
                    {
                        main.Dispatcher.Invoke(new Action(() =>
                        {
                            if (PortInUse(8190))
                            {
                                strblock = "时间：" + DateTime.Now.ToString() + "端口被占用 \n";
                                main.Txtblock.Text += strblock;
                                Log.Debug(strblock);
                            }
                            else
                            {
                                KillProcess("cmd");
                                Process.Start(path);
                                strblock = "时间：" + DateTime.Now.ToString() + "程序已重启 \n";
                                main.Txtblock.Text += strblock;
                                Log.Debug(strblock);
                            }
                        }));
                    }

                    Thread.Sleep(5000);

                });
            }
        }

        /// <summary>
        /// 检测端口是否占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            { // www.jbxue.com
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        /// <summary>
        /// 关闭指定进程
        /// </summary>
        /// <param name="strProcessesByName"></param>
        public static void KillProcess(string strProcessesByName)//关闭线程
        {
           
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == strProcessesByName)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(); // possibly with a timeout
                    }
                    catch (System.ComponentModel.Win32Exception e)
                    {
                        Log.Debug(e.Message.ToString());   // process was terminating or can't be terminated - deal with it
                    }
                    catch (InvalidOperationException e)
                    {
                        Log.Debug(e.Message.ToString()); // process has already exited - might be able to let this one go
                    }
                }
            }


        }

        private void Closeexe_Click(object sender, RoutedEventArgs e)
        {
            main.Dispatcher.Invoke(new Action(() =>
            {
                this.Closeexe.Visibility = Visibility.Hidden;
                ISread = false;
            }));
        }
    }
}
