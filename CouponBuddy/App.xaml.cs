using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using CouponBuddy.Api;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Navigation;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using IContainer = Autofac.IContainer;

namespace CouponBuddy
{
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        public App()
        {
            InitializeComponent();
            ConfigureServices();
            MainWindow = new MainWindow();
        }

        private void ConfigureServices()
        {
            string baseUrl = CouponBuddy.Properties.Resources.BASE_URL;
            var builder = new ContainerBuilder();

            builder.RegisterType<DatabaseManager>()
                 .WithParameter(new TypedParameter(typeof(string), baseUrl))
                 .As<IDatabaseManager>()
                 .SingleInstance();

            builder.RegisterType<BitmapImageLoader>()
                 .AsSelf()
                 .SingleInstance();

            builder.RegisterType<Navigation.NavigationService>()
                .As<INavigationService>()
                .SingleInstance();

            Container = builder.Build();
            var csl = new AutofacServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}
