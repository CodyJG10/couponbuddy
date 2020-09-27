using CouponBuddy.ViewModels.ActiveScreen;
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

namespace CouponBuddy.Views.ActiveScreen
{
    public partial class ActiveScreen : Page
    {
        private ActiveScreenViewModel Context
        {
            get { return DataContext as ActiveScreenViewModel; }
        }

        public ActiveScreen()
        {
            InitializeComponent();
            DataContext = new ActiveScreenViewModel();
        }

        private void BtnAdvertiseWithUs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AdvertiseWithUs.AdvertiseWithUs page = new AdvertiseWithUs.AdvertiseWithUs();
            MainWindow.Instance.NavigateToPage(page);
        }

        //Point scrollMousePoint = new Point();
        //double hOff = 1;
        //private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    scrollMousePoint = e.GetPosition(scrollViewer);
        //    hOff = scrollViewer.VerticalOffset;
        //    scrollViewer.CaptureMouse();
        //}

        //private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (scrollViewer.IsMouseCaptured)
        //    {
        //        scrollViewer.ScrollToVerticalOffset(hOff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y));
        //    }
        //}

        //private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    scrollViewer.ReleaseMouseCapture();
        //}

        //private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    scrollViewer.ScrollToHorizontalOffset(scrollViewer.VerticalOffset + e.Delta);
        //}
    }
} 