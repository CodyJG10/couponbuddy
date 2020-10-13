using CommonServiceLocator;
using CouponBuddy.Navigation;
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

namespace CouponBuddy.Views.Shared
{
    /// <summary>
    /// Interaction logic for BackButton.xaml
    /// </summary>
    public partial class BackButton : UserControl
    {
        public BackButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SetFontSizeProperty =
            DependencyProperty.Register("ButtonFontSize", typeof(double), typeof(BackButton),
                new PropertyMetadata(0.0D, new PropertyChangedCallback(OnFontSizeChanged)));

        public double ButtonFontSize
        {
            get { return (double)GetValue(SetFontSizeProperty); }
            set { SetValue(SetFontSizeProperty, value); }
        }

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BackButton control = d as BackButton;
            control.OnFontSizeChanged(e);
        }

        private void OnFontSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            ButtonFontSize = (double)e.NewValue;
            //btnBack.FontSize = ButtonFontSize;
        }

        private void BtnBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            navigation.Back();
        }
    }
}
