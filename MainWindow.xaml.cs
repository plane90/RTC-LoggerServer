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

namespace RTC_LoggerServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnWindowStateChanged(this, EventArgs.Empty);
        }

        private void OnMouseDownListBoxTitle(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void OnClick_BtnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OnClick_BtnMinimalize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void OnClick_BtnMaximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
        private void OnClick_BtnNormalize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }
        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // hidden 대신 collapsed를 사용하면 투명 공간을 남기지 않는다.
                BtnMaximize.Visibility = Visibility.Collapsed;
                BtnNormalize.Visibility = Visibility.Visible;
            }
            else
            {
                BtnMaximize.Visibility = Visibility.Visible;
                BtnNormalize.Visibility = Visibility.Collapsed;
            }
        }
    }
}
