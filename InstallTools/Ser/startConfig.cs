using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace  InstallTools.Ser
{
    public class startConfig
    {
        public static Dictionary<String, JToken> arryData = new Dictionary<String, JToken>();
        public startConfig()
        {
            Loginit();
            Only();
            //selfstart();
           // shortcut();
           // DataFileJson();
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_RESTORE = 0xF120;
        //防止进程多开
        public static bool Only()
        {
            bool flag = false;
            try
            {
                Process cur = Process.GetCurrentProcess();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.Id == cur.Id) continue;
                    if (p.ProcessName == cur.ProcessName)
                    {
                        MessageBox.Show("程序已运行，禁止重复开启");
                        System.Environment.Exit(0);
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                Log.Debug("线程操作异常：", ex.Message);
                return flag;
            }

        }

        //创建开机自启
        public static void selfstart()
        {
            try
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\Start.bat";
                FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(fs1);
                string write = "@echo off\ncd /d %~dp0\n%1 start \"\" mshta vbscript:createobject(\"shell.application\").shellexecute(\"\"\"%~0\"\"\",\"::\",,\"runas\",1)(window.close)&exit\nstart " + System.Windows.Forms.Application.ExecutablePath;
                sw.WriteLine(write);//开始写入值
                sw.Close();
                fs1.Close();
            }
            catch (Exception ex)
            {
                Log.Debug("创建开机自启：", ex.Message);
            }
        }

        //创建桌面快捷方式
        public static void shortcut()
        {
            try
            {
                //快捷方式的完全限定路径
                String lnkFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\" + ConfigurationManager.AppSettings["LnkName"] + ".lnk";
                //快捷方式启动程序时需要使用的参数
                string args = "";
                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(shellType);
                var shortcut = shell.CreateShortcut(lnkFilePath);
                shortcut.TargetPath = Assembly.GetEntryAssembly().Location;
                shortcut.Arguments = args;
                
                shortcut.IconLocation = System.AppDomain.CurrentDomain.BaseDirectory + "play.ico"; ;
                shortcut.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Log.Debug("创建快捷方：", ex.Message);
            }
        }

        public void Loginit()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            // .WriteTo.Console()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(DateTime.Now.ToString("yyyyMM") + "logs", $"log.txt"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        }

        public static string GetData(string FileName)
        {
            String FliePath = System.IO.Directory.GetCurrentDirectory() + "\\" + FileName;
            string json = "";
            if (File.Exists(FliePath))
            {
                StreamReader MyReader = null;
                try
                {
                    MyReader = new StreamReader(FliePath, System.Text.Encoding.GetEncoding("utf-8"));
                    json = MyReader.ReadToEnd();
                    if (MyReader != null)
                    {
                        MyReader.Close();
                    }
                }
                catch (Exception)
                {

                }
            }
            return json;
        }

        public void DataFileJson() {
            JToken data = JToken.Parse( GetData("encircle.json"));
            foreach (var item in data)
            {
                arryData.Add(item["centerCode"].ToString(), item["peripheries"]);
            }
           
        }


    }

  
}
