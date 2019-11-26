using BrochureBuddy.ViewModels.ActiveScreen;
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

namespace BrochureBuddy.Views.ActiveScreen
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

        private void CategoryButtonClicked(object sender, MouseButtonEventArgs e)
        {
            CategoryButtonView btn = sender as CategoryButtonView;
            int category = int.Parse(btn.CategoryID);
            Context.ShowCategory(category);
        }

        private void BtnAdvertiseWithUs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AdvertiseWithUs.AdvertiseWithUs page = new AdvertiseWithUs.AdvertiseWithUs();
            MainWindow.Instance.NavigateToPage(page);
        }
    }
} 