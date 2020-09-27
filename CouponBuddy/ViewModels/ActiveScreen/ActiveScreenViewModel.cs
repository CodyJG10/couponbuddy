using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommonServiceLocator;
using CouponBuddy.Controllers;
using CouponBuddy.Entities;
using CouponBuddy.Navigation;
using CouponBuddy.Views.ActiveScreen;

namespace CouponBuddy.ViewModels.ActiveScreen
{
    public class ActiveScreenViewModel : ViewModel
    {
        private INavigationService _navigation;

        public List<CategoryButtonView> CategoryButtons { get; set; } = new List<CategoryButtonView>();

        public ActiveScreenViewModel()
        {
            _navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            InitButtons();
        }

        public void ShowCategory(int category)
        {
            Views.VendorListScreen.VendorListScreen page = new Views.VendorListScreen.VendorListScreen(category);
            _navigation.Navigate(page);
        }

        private void InitButtons() 
        {
            var fields = typeof(Categories).GetFields();
            foreach (var field in fields)
            {
                int fieldValue = (int)field.GetValue(typeof(Categories));
                if (!HasVendorInCategory(fieldValue))
                    continue;
                var categoryInfo = Categories.GetCategory(fieldValue);
                string filePath = "/Assets/" + categoryInfo.Picture;
                CategoryButtonView view = new CategoryButtonView();
                view.ButtonImage = filePath;
                view.CategoryID = fieldValue.ToString();
                view.MouseLeftButtonDown += (obj, args) =>
                {
                    CategoryButtonView btn = obj as CategoryButtonView;
                    int category = int.Parse(btn.CategoryID);
                    ShowCategory(category);
                };
                view.Style = Application.Current.FindResource("CategoryButton") as Style;
                CategoryButtons.Add(view);
            }
        }

        private bool HasVendorInCategory(int categoryId) 
        {
            var vendors = VendorController.Instance.GetAllVendors();
            return vendors.Any(x => x.CategoryId == categoryId);
        }
    }
}