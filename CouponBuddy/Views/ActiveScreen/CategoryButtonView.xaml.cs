using CouponBuddy.Entities;
using CouponBuddy.Entities.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class CategoryButtonView : UserControl
    {
        public static readonly DependencyProperty SetCategoryIDProperty =
            DependencyProperty.Register("CategoryID", typeof(string), typeof(CategoryButtonView),
                new PropertyMetadata("", new PropertyChangedCallback(OnCategoryIDChanged)));

        public static readonly DependencyProperty SetButtonImageProperty =
            DependencyProperty.Register("ButtonImage", typeof(string), typeof(CategoryButtonView),
                new PropertyMetadata("", new PropertyChangedCallback(OnButtonImageChanged)));

        public string CategoryID
        {
            get { return (string)GetValue(SetCategoryIDProperty); }
            set { SetValue(SetCategoryIDProperty, value); }
        }

        public string ButtonImage
        {
            get { return (string)GetValue(SetButtonImageProperty); }
            set { SetValue(SetButtonImageProperty, value); }
        }

        public CategoryButtonView()
        {
            InitializeComponent();
        }

        #region ID
        private static void OnCategoryIDChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            CategoryButtonView control = d as CategoryButtonView;
            control.OnCategoryIDChanged(e);
        }

        private void OnCategoryIDChanged(DependencyPropertyChangedEventArgs e)
        {
            int id = int.Parse(((string)e.NewValue));
            SetCategoryName(id);
        }

        private void SetCategoryName(int newId)
        {
            var fields = typeof(Categories).GetFields();
            foreach (var field in fields)
            {
                int value = (int)field.GetValue(field);
                if (value == newId)
                {
                    var info = field.GetCustomAttribute(typeof(CategoryInfo)) as CategoryInfo;
                    string categoryName = info.DisplayName;
                    lblCategoryName.Content = categoryName;
                }
            }
        }
        #endregion

        #region Button Image

        private static void OnButtonImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CategoryButtonView control = d as CategoryButtonView;
            control.OnButtonImageChanged(e);
        }

        private void OnButtonImageChanged(DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            SetButtonImage(value);
        }

        private void SetButtonImage(string path)
        {
            string uriPath = AppDomain.CurrentDomain.BaseDirectory + path;
            buttonImage.ImageSource = new BitmapImage(new Uri(uriPath)) as ImageSource;
        }

        #endregion
    }
}