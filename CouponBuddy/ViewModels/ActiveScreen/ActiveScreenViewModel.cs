using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonServiceLocator;
using CouponBuddy.Navigation;

namespace CouponBuddy.ViewModels.ActiveScreen
{
    public class ActiveScreenViewModel : ViewModel
    {
        private INavigationService _navigation;

        public ActiveScreenViewModel()
        {
            _navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
        }

        public void ShowCategory(int category)
        {
            Views.VendorListScreen.VendorListScreen page = new Views.VendorListScreen.VendorListScreen(category);
            _navigation.Navigate(page);
        }
    }
}