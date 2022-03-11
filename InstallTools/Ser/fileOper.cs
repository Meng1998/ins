using ICSharpCode.SharpZipLib.Zip;
using InstallTools.model;
using Ookii.Dialogs.WinForms;
using Serilog;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace InstallTools.Ser
{
    class fileOper
    {

        public static String filepath;
        public static SqlSugarClient db;
        private static Boolean sqlsugIsread = false;

        public static Boolean sqlInit(String IP, String database, String pwd)
        {
           
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"PORT=5432;DATABASE={database};HOST={IP};PASSWORD={pwd};USER ID=postgres",
                DbType = DbType.PostgreSQL,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
            try
            {
                db.Open();
                sqlsugIsread = true;
                return sqlsugIsread;
            }
            catch (Exception)
            {
                sqlsugIsread = false;
                return sqlsugIsread;
            }
        }

        #region 解压文件
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="zipFilePath">压缩文件所在目录</param>
        ///  <param name="unzippath">解压文件所在目录</param>
        public bool UnZipFile(string zipFilePath, string unzippath)
        {
            try
            {
                ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(unzippath);
                    string fileName = Path.GetFileName(theEntry.Name);

                    //生成解压目录

                    Directory.CreateDirectory(directoryName);

                    if (fileName != String.Empty)
                    {
                        //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入

                        if (theEntry.CompressedSize == 0)
                            break;
                        //解压文件到指定的目录

                        directoryName = Path.GetDirectoryName(unzippath + "\\" + theEntry.Name);
                        //建立下面的目录和子目录

                        Directory.CreateDirectory(directoryName);

                        FileStream streamWriter = File.Create(unzippath + "\\" + theEntry.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //strJYtime = (DateTime.Now - dt_start).ToString();
            //MessageBox.Show("解压时间："+strJYtime);
        }


        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径，全路径格式</param>  
        /// <param name="unZipDir">解压文件存放路径,全路径格式，为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <param name="err">出错信息</param> 
        /// <returns>解压是否成功</returns>  
        public static bool UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new System.IO.FileNotFoundException("压缩文件不不能为空！");
            }

            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件: " + zipFilePath + " 不存在!");
            }

            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  

            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), "");
            if (!unZipDir.EndsWith("//"))
                unZipDir += "//";

            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                            directoryName += "//";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }//while  
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;

        }//解压结束 
        #endregion


        #region 选择文件生成TreeView
        public static Boolean FilePathname()
        {
            try
            {
                VistaFolderBrowserDialog vistaOpenFileDialog = new VistaFolderBrowserDialog();
                if (vistaOpenFileDialog.ShowDialog()== DialogResult.OK)
                {
                    filepath = vistaOpenFileDialog.SelectedPath;
                } 

                //SaveFileDialog saveDlg = new SaveFileDialog();
                //if (saveDlg.ShowDialog() == DialogResult.OK)
                //{
                //    //String a = saveDlg.Filter();
                //}
              
                //System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
                //openFileDialog.Description = "Please choose the path";
                //if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                //{
                //    filepath = openFileDialog.SelectedPath;
                //}
                //DirectoryInfo di = new DirectoryInfo(filepath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static List<string> list1 = new List<string>();
        public static List<string> list2 = new List<string>();
        /// <summary>
        /// 生成树形结构数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tree1"></param>
        public static Boolean BindTreeView2(string path, TreeViewItem tree1)
        {
           
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] dirs = di.GetDirectories();
            foreach (var item in dirs)
            {
                list2.Add(item.ToString());
            }
            //string abc = dirs[1].ToString();
            //  System.Windows.Controls.TreeView root = new System.Windows.Controls.TreeView();

            //  Regex rex = new Regex(@"^\d+$");
            try
            {
                if (dirs.Length > 10)
                {
                    Array.Sort(dirs, (x1, x2) => int.Parse(Regex.Match(x1.Name, @"\d+").Value).CompareTo(int.Parse(Regex.Match(x2.Name, @"\d+").Value)));
                }
                foreach (DirectoryInfo i in dirs)
                { //将递归遍历得到的文件夹路径与treeviewitem节点进行对应,并动态创建treeviewitem的Selected事件(选中事件),触发Selected事件,将该目录下得到的所有文件夹和文件路径添加到list1集合,若在文件夹之下遍历到子文件夹则创建子节点与子文件夹对应

                    TreeViewItem ziDt = new TreeViewItem();
                    ziDt.Header = i.Name;
                    tree1.Items.Add(ziDt);

                    ziDt.Selected += new RoutedEventHandler(delegate (object shabi, RoutedEventArgs r)
                    {  //选中节点，通过 MessageBox.Show打印 节点对应文件夹下的所有文件夹和文件路径
                        list1.Clear(); //清空之前选中节点所取得的所有路径
                        string c = null;
                        string[] directory1 = Directory.GetDirectories(i.FullName);
                        foreach (string a in directory1)  //将目录下的文件夹路径加到list1
                        {
                            list1.Add(a);
                        }
                        //FileInfo[] fileInfos = i.GetFiles("tileset.json");
                        //if (fileInfos.Length>0)
                        //{
                        string[] file1 = Directory.GetFiles(i.FullName);
                        foreach (string a in file1)      //将目录下的文件路径加到list1
                        {
                            list1.Add(a);
                        }
                        //  }
                        foreach (string a in list1)
                        {
                            c = c + "\r\n" + a;
                        }


                    });
                    BindTreeView2(i.FullName, ziDt);

                }
                //this.Dispatcher.Invoke(new Action(() =>
                //{
                //    Probar.Visibility = Visibility.Hidden;
                //}));
                return true;
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("报错：" + ex.Message);
                return false;
            }
        }
        #endregion



        #region 文件导入数据库


        public static Boolean DataIns()
        {
            Boolean inf = false;
            if (!sqlsugIsread)
                return inf;
            DirectoryInfo di = new DirectoryInfo(filepath);
            DirectoryInfo[] dirlist = di.GetDirectories();
            foreach (DirectoryInfo item in dirlist)
            {
                foreach (var child in new DirectoryInfo(item.FullName).GetDirectories())
                {
                    DataInsModel(item.FullName + "\\3dtiles", child.Name, item.Name,ref inf);
                }
            }
            return inf;
        }

        static List<map_build> build_list = new List<map_build>();
        static List<map_floor> floor_list = new List<map_floor>();
        static List<map_model> model_list = new List<map_model>();
        public static Boolean DataInsModel(String file, String nodetype, String datatype,ref Boolean dataExecut)
        {
            String err = "";
            try
            {
                foreach (var item in new DirectoryInfo(file).GetDirectories())
                {
                    map_build map_Build = new map_build();
                    map_floor map_Floor = new map_floor();
                    map_model map_Model = new map_model();
                    if (IsreadJson(item.FullName))
                    {
                        model_list.Add(new map_model
                        {
                            id = $"{modelNamestr(item.Name.Split('.')[0])}_normal",
                            pid = "0_normal",
                            node_name = item.Name.Split('.')[0],
                            node_type = nodetype,
                            data_type = datatype,
                            data_url = $"$serverURL$/3dtiles/{item.Name.Split('.')[0]}/tileset.json",
                            visible = false
                        });
                    }
                    else
                    {

                        model_list.Add(new map_model
                        {
                            id = $"{modelNamestr(item.Name.Split('.')[0])}_normal",
                            pid = "0_normal",
                            node_name = item.Name.Split('.')[0],
                            node_type = "group",
                            data_type = datatype,
                            data_url = null,
                            visible = false
                        });


                        foreach (var modelchild in new DirectoryInfo(item.FullName).GetDirectories())
                        {
                            if (IsreadJson(modelchild.FullName))
                            { err += "建筑文件格式异常,路径：" + modelchild.FullName; }
                            else
                            {
                                model_list.Add(new map_model
                                {
                                    id = $"100005_{modelchild.Name.Split('.')[0]}_normal",
                                    pid = "100005_normal",
                                    node_name = modelchild.Name.Split('.')[0],
                                    node_type = "group",
                                    data_type = datatype,
                                    data_url = null,
                                    visible = false
                                });

                                build_list.Add(new map_build
                                {
                                    id = $"100005_{modelchild.Name.Split('.')[0]}",
                                    group_id = $"100005_{modelchild.Name.Split('.')[0]}",
                                    build_name = $"建筑{modelchild.Name.Split('.')[0]}"
                                });


                                foreach (var floorchild in new DirectoryInfo(modelchild.FullName).GetDirectories())
                                {
                                    if (IsreadJson(floorchild.FullName))
                                    {
                                        model_list.Add(new map_model
                                        {
                                            id = $"100005_{modelchild.Name.Split('.')[0]}_{modelurlstr(floorchild.Name.Split('.')[0])}_normal",
                                            pid = $"100005_{modelchild.Name.Split('.')[0]}_normal",
                                            node_name = floorchild.Name.Split('.')[0],
                                            node_type = nodetype,
                                            data_type = datatype,
                                            data_url = $"$serverURL$/3dtiles/JZ/{modelchild.Name.Split('.')[0]}/{floorchild.Name.Split('.')[0]}/tileset.json",
                                            visible = false
                                        });
                                        
                                        floor_list.Add(new map_floor
                                        {
                                            id = $"100005_{modelchild.Name.Split('.')[0]}_{modelurlstr(floorchild.Name.Split('.')[0])}",
                                            group_id = $"100005_{modelchild.Name.Split('.')[0]}_{modelurlstr(floorchild.Name.Split('.')[0])}",
                                            order_num = Int32.Parse(floorchild.Name.Split('F')[1]),
                                            floor_name = floorchild.Name.Split('.')[0],
                                            build_id = $"100005_{modelchild.Name.Split('.')[0]}",
                                            //model_url = $"$serverURL$/3dtiles/JZ/{modelchild.Name.Split('.')[0]}/{floorchild.Name.Split('.')[0]}/tileset.json",
                                        });
                                    }
                                    else
                                    {
                                        err += "楼层文件格式异常,路径：" + floorchild.FullName;
                                    }

                                }
                            }
                        }
                    }
                }
                //开启事务
                db.BeginTran();
                //清理表
                db.Deleteable<map_build>().ExecuteCommand();
                db.Deleteable<map_floor>().ExecuteCommand();
                db.Deleteable<map_model>().ExecuteCommand();

                //插入数据
                db.Insertable(build_list.ToArray()).ExecuteCommand();
                db.Insertable(floor_list.ToArray()).ExecuteCommand();
                db.Insertable(model_list.ToArray()).ExecuteCommand();
                //提交事务
                db.CommitTran();
                dataExecut = true;
                return dataExecut;
            }
            catch (Exception ex)
            {
                //回滚事务
                db.RollbackTran();
                err += "空间名：" + ex.Source + "；" + '\n' +
                                  "方法名：" + ex.TargetSite + '\n' +
                                  "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                                  "错误提示：" + ex.Message;
                Log.Debug(err);
                dataExecut = false;
                return dataExecut;
            }
        }


        public static Boolean IsreadJson(String itemfile)
        {
            if (Directory.GetFiles(itemfile, "*.json").Length > 0)
                return true;
            else
                return false;
        }
        public static String modelNamestr(String name)
        {
            String str = "";
            switch (name)
            {
                case "DX":
                    str = "100001";
                    break;
                case "LH":
                    str = "100002";
                    break;
                case "SWJZ":
                    str = "100003";
                    break;
                case "XP":
                    str = "100004";
                    break;
                case "JZ":
                    str = "100005";
                    break;
                default:
                    break;
            }
            return str;
        }

        public static String modelurlstr(String strname)
        {
            if (strname.Split('F')[1].Length < 2)
            {
                return "0" + strname.Split('F')[1];
            }
            else
            {
                return strname.Split('F')[1];
            }
        }

        #endregion
    }
}
