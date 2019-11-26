using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BrochureBuddy.Navigation
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        public void Back()
        {
            _frame.NavigationService.GoBack();
        }

        public void Navigate(Type source, object parameter = null)
        {
            var src = Activator.CreateInstance(source, parameter);
            _frame.Navigate(src, parameter);
        }

        public void Navigate(Page page)
        {
            _frame.Navigate(page);
        }

        public void SetMainFrame(Frame frame)
        {
            _frame = frame;
        }
    }
}