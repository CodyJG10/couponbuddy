using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CouponBuddy.Views.Coupon
{
    public partial class CouponSentScreen : Page
    {
        public CouponSentScreen()
        {
            InitializeComponent();
            ReturnToInactive();
        }

        private void ReturnToInactive()
        {
            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += (obj, e) =>
            {
<<<<<<< HEAD
                Application.Current.Dispatcher.Invoke(() => 
=======
                Application.Current.Dispatcher.Invoke(() =>
>>>>>>> master
                {
                    MainWindow.Instance.NavigateToPage(new InactiveScreen.InactiveScreen());
                });
                timer.Dispose();
            };
            timer.Start();
        }
    }
}