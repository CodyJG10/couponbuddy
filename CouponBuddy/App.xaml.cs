﻿using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CouponBuddy.Properties;
using CouponBuddy.Util;
using CommonServiceLocator;
using CouponBuddy.Api;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Navigation;
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
using CouponBuddy.Admin;
using CouponBuddy.Entities;

namespace CouponBuddy
{
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        public bool IsFirstLaunch { get; private set; }

        public App()
        {
            InitializeComponent();
            ConfigureServices();

            IsFirstLaunch = Settings.Default.FIRST_LAUNCH;
            //IsFirstLaunch = true;

            //IsFirstLaunch = true;

            //#if DEBUG
            //            IsFirstLaunch = false;
            //#endif

            if (!IsFirstLaunch)
            { 
                MainWindow = new MainWindow();
            }
            else
            {
                MainWindow = new FirstLaunch();
            }
            MainWindow.Show();
        }

        private void ConfigureServices()
        {
            string baseUrl = CouponBuddy.Properties.Resources.BASE_URL;
            var builder = new ContainerBuilder();

            string username = Settings.Default.DATABASE_USERNAME;
            string password = Settings.Default.DATABASE_PASSWORD;
            builder.RegisterType<DatabaseManager>()
                .WithParameter(new NamedParameter("database", baseUrl))
                .WithParameter(new NamedParameter("username", username))
                .WithParameter(new NamedParameter("password", password))
                .As<IDatabaseManager>()
                .SingleInstance();

            string storageConnectionString = CouponBuddy.Properties.Resources.STORAGE_CONNECTION_STRING;
            builder.RegisterType<ImageLoader>()
                .AsSelf()
                .WithParameter(new NamedParameter("connectionString", storageConnectionString));

            builder.RegisterType<Navigation.NavigationService>()
                .As<INavigationService>()
                .SingleInstance();

            Container = builder.Build();
            var csl = new AutofacServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}