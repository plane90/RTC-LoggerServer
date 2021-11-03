using System;
using System.Windows;
using System.Windows.Input;

namespace RTC_LoggerServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int MyProperty { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            OnWindowStateChanged(this, EventArgs.Empty);
            Net.Server.RunServer();
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
