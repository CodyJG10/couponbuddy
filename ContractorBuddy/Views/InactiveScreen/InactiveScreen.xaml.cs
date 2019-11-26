using CommonServiceLocator;
using BrochureBuddy.Navigation;
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
using System.Windows.Media.Animation;

namespace BrochureBuddy.Views.InactiveScreen
{
    /// <summary>
    /// Interaction logic for InactiveScreen.xaml
    /// </summary>
    public partial class InactiveScreen : Page
    {
        public InactiveScreen()
        {
            InitializeComponent();
            BeginAnimation();
        }

        private void BeginAnimation()
        {
            //ThicknessAnimation bounceAnimation = new ThicknessAnimation();
            //BounceEase BounceOrientation = new BounceEase();
            //BounceOrientation.Bounces = 4;
            //BounceOrientation.Bounciness = 2;
            //bounceAnimation.To = new Thickness(143, 200, 0, 0);
            //bounceAnimation.From = new Thickness(143, 0, 0, 0);
            //bounceAnimation.EasingFunction = BounceOrientation;
            //imgClickMeTransform.BeginAnimation(MarginProperty, bounceAnimation);

            //DoubleAnimation animation = new DoubleAnimation();
            //animation.From = 200;
            //animation.To = 200;
            //animation.Duration = new Duration(TimeSpan.FromSeconds(3));
            //animation.AutoReverse = true;
            //imgClickMe.BeginAnimation(Image.RenderTransformProperty, animation);

            //Storyboard storyboard = new Storyboard();
            //ThicknessAnimation animation = new ThicknessAnimation();
            //animation.From = new Thickness(-this.WindowWidth, -this.WindowHeight, this.WindowWidth, this.WindowHeight);
            //animation.To = new Thickness(-this.WindowWidth - 200, -this.WindowHeight + (this.WindowHeight / 2), this.WindowWidth - 200, this.WindowHeight - (this.WindowHeight / 2));
            //animation.RepeatBehavior = RepeatBehavior.Forever;
            //Storyboard.SetTargetProperty(storyboard, new PropertyPath("Margin"));
            //storyboard.Children.Add(animation);
            //storyboard.Begin();

            //DoubleAnimation rightAnimation = new DoubleAnimation();
            //rightAnimation.From = imgClickMe.Margin.Right;
            //rightAnimation.From = this.WindowWidth / 2; 

            //ThicknessAnimation leftAnimation = new ThicknessAnimation();
            //Thickness originalMargin = imgClickMe.Margin;
            //leftAnimation.From = originalMargin;
            //Thickness newLeftMargin = originalMargin;
            //newLeftMargin.Right = this.Width;
            //leftAnimation.To = newLeftMargin;
            //leftAnimation.Duration = new Duration(TimeSpan.FromSeconds(3));
            //leftAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //imgClickMe.BeginAnimation(Image.MarginProperty, leftAnimation);
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var _navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            Application.Current.Dispatcher.Invoke((Action)delegate {
                var screen = new ActiveScreen.ActiveScreen();
                _navigation.Navigate(screen);
            });
        }
    }
}
