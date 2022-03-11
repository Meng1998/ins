using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// CSMvvm.xaml 的交互逻辑
    /// </summary>
    public partial class CSMvvm : Window
    {

        public CSMvvm()
        {
            //对静态对象进行赋值
            Student stu = new Student { StudentID = 20130602, Name = "李四", EntryDate = DateTime.Parse("2012-09-15"), Credit = 40.5, Txtval = "Hellow" };
            GlobalData.student = stu;
            InitializeComponent();
            //实例对象数据源
           // stackPanel1.DataContext = new Student() { StudentID = 20130501, Name = "张三", EntryDate = DateTime.Parse("2013-09-01"), Credit = 0.0, Txtval = "Hellow" };

        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Button btn = e.OriginalSource as Button;
            if (btn != null)
            {
                switch (btn.Name)
                {
                    case "btn_new":
                        {
                            Student stu = (Student)stackPanel1.DataContext;
                            stu.Name = "张三三";
                            stu.Credit = 34.5;

                        }
                        break;
                    case "btn_static":
                        {
                            GlobalData.student.Name = "李四四";
                            GlobalData.student.EntryDate = DateTime.Now;
                        }
                        break;
                    case "btn_resource":
                        {
                            Student stu = (Student)this.FindResource("stuKey");
                            stu.Name = "王六六";
                            stu.StudentID = 20140025;

                            WinMvvvm win = new WinMvvvm();
                            win.Show();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public class GlobalData
    {
        public static Student student = null;
    }

    public class Student : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public Student()
        {
            
        }
        private String txtval;
        /// <summary>
        /// 文本
        /// </summary>
        public String Txtval
        {
            get { return txtval; }
            set
            {
                if (txtval == value)
                    return;
                txtval = value;
                OnPropertyChanged("Txtval");
            }
        }
        private int studentID;
        /// <summary>
        /// 学号
        /// </summary>
        public int StudentID
        {
            get { return studentID; }
            set
            {
                if (studentID == value)
                    return;
                studentID = value;
                OnPropertyChanged("StudentID");
            }
        }
        private string name;
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private DateTime entryDate;
        /// <summary>
        /// 入学日期
        /// </summary>
        public DateTime EntryDate
        {
            get { return entryDate; }
            set
            {
                if (entryDate == value)
                    return;
                entryDate = value;
                OnPropertyChanged("EntryDate");
            }
        }

        private double credit;
        /// <summary>
        /// 学分
        /// </summary>
        public double Credit
        {
            get { return credit; }
            set
            {
                if (credit == value)
                    return;
                credit = value;
                OnPropertyChanged("Credit");
            }
        }


    }


}


