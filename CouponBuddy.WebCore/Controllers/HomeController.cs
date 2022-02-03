using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CouponBuddy.Web.Models;
using Microsoft.AspNetCore.Identity;
using CouponBuddy.Web.Areas.Identity;

namespace CouponBuddy.Web.Controllers
{
    public class HomeController : Controller
    {

        //private RoleManager<IdentityRole> _roleManager;
        //private UserManager<User> _userManager;

        //public HomeController(RoleManager<IdentityRole> roleManager,
        //    UserManager<User> userManager)
        //{
        //    _roleManager = roleManager;
        //    _userManager = userManager;
        //}

        public IActionResult Index()
        {
            //_roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole("LocationManager")).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole("Vendor")).GetAwaiter().GetResult();

            //if (User.Identity.IsAuthenticated)
            //{
            //    var user = _userManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult();
            //    _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            //    return Content("Success");
            //}

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";


            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
