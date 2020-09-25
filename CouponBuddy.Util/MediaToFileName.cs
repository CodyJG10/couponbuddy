using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Util
{
    public static class MediaToFileName
    {
        public static string GetFileName(string container, string file)
        {
            return container + "_" + file;
        }
    }
}