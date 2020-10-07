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
using System.Windows.Shapes;

namespace CouponBuddy.Admin
{
    public partial class FirstLaunch : Window
    {
        public FirstLaunch()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtAdminUsername.Text;
                string password = txtAdminPassword.Text;
                int adDuration = int.Parse(txtInactiveAdDuration.Text);
                int timeout = int.Parse(txtInactivityTimeout.Text);
                string locationId = txtLocationId.Text;
                int deviceId = int.Parse(txtDeviceId.Text);

                Properties.Settings.Default["DATABASE_USERNAME"] = username;
                Properties.Settings.Default["DATABASE_PASSWORD"] = password;
                Properties.Settings.Default["INACTIVE_AD_DURATION"] = adDuration;
                Properties.Settings.Default["INACTIVITY_TIMEOUT"] = timeout;
                Properties.Settings.Default["LOCATION_ID"] = locationId;
                Properties.Settings.Default["DEVICE_ID"] = deviceId;
                Properties.Settings.Default["FIRST_LAUNCH"] = false;

                Properties.Settings.Default.Save();

                MessageBox.Show("Initialized, please restart.");
                Application.Current.Shutdown();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
