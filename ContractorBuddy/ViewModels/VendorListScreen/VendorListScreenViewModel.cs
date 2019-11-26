using CommonServiceLocator;
using BrochureBuddy.Api.Interfaces;
using BrochureBuddy.Api.Managers;
using BrochureBuddy.Controllers;
using BrochureBuddy.Entities;
using BrochureBuddy.Entities.Metadata;
using BrochureBuddy.Navigation;
using BrochureBuddy.Views.VendorListScreen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BrochureBuddy.ViewModels.VendorListScreen
{
    public class VendorListScreenViewModel : ViewModel
    {
        public string Title { get; set; }
        public ObservableCollection<Vendor> Vendors { get; set; } = new ObservableCollection<Vendor>();
        public ObservableCollection<VendorButton> VendorButtons { get; set; } = new ObservableCollection<VendorButton>();

        private readonly IDatabaseManager _database;
        private readonly INavigationService _navigation;
        private readonly int _category;

        public VendorListScreenViewModel(int category)
        {
            _category = category;
            _database = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;
            _navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            SetTitle();
            LoadVendors();
        }

        private void SetTitle()
        {
            var fields = typeof(Categories).GetFields();
            foreach (var field in fields)
            {
                int value = (int)field.GetValue(field);
                if (value == _category)
                {
                    var info = field.GetCustomAttribute(typeof(CategoryInfo)) as CategoryInfo;
                    Title = info.VendorListScreenTitle;
                }
            }
        }

        private void LoadVendors()
        {
            var vendors = VendorController.Instance.GetAllVendors();
            foreach (Vendor vendor in vendors)
            {
                if (vendor.CategoryId == _category)
                {
                    Vendors.Add(vendor);
                    var vendorButton = new VendorButton()
                    {
                        Vendor = vendor
                    };
                    VendorButtons.Add(vendorButton);
                    _database.AddImpression(vendor, Properties.Resources.LOCATION_ID);
                }
            }
        }
    }
}