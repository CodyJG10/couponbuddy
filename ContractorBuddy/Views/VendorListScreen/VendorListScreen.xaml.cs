using BrochureBuddy.Entities;
using BrochureBuddy.ViewModels.VendorListScreen;
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

namespace BrochureBuddy.Views.VendorListScreen
{
    public partial class VendorListScreen : Page
    {
        private VendorListScreenViewModel Context { get { return DataContext as VendorListScreenViewModel; } }

        public VendorListScreen(int category)
        {
            InitializeComponent();
            DataContext = new VendorListScreenViewModel(category);
        }
    }
}