using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CouponBuddy.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type source, object parameter = null);
        void Navigate(Page page);
        void Back();
        void SetMainFrame(Frame frame);
    }
}
