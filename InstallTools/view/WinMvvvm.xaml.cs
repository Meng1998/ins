using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// WinMvvvm.xaml 的交互逻辑
    /// </summary>
    public partial class WinMvvvm : Window
    {
        public WinMvvvm()
        {
            InitializeComponent();
            this.DataContext = model;

        }
        WPFMVVMExample model = new WPFMVVMExample();
       

        private void Button_Click_red(object sender, RoutedEventArgs e)
        {
            model.color = WPFColor.red;
        }
        private void Button_Click_yellow(object sender, RoutedEventArgs e)
        {
            model.color = WPFColor.yellow;
        }
        private void Button_Click_blue(object sender, RoutedEventArgs e)
        {
            model.color = WPFColor.blue;
        }

    }

    public class WPFMVVMExample : INotifyPropertyChanged
    {

        private WPFColor _color;
        public WPFColor color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                NotifyPropertyChanged("color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public enum WPFColor
    {
        red,
        yellow,
        blue
    }

    public class MyConverter : IValueConverter
    {
        /// <summary>
        /// 源到目标
        /// </summary>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                if ((parameter as string).Equals("red") && ((WPFColor)value) == WPFColor.red)
                {
                    return Visibility.Visible;
                }
                else if ((parameter as string).Equals("yellow") && ((WPFColor)value) == WPFColor.yellow)
                {
                    return Visibility.Visible;
                }
                else if ((parameter as string).Equals("blue") && ((WPFColor)value) == WPFColor.blue)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            else
            {
                // 无法解析value时隐藏
                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// 目标到源
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


}
