using AutoMapper;
using MahApps.Metro.Controls;
using ServerManagement.Identity;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.View;
using ServerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ServerManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Mapper.Initialize(q =>
            {
                q.CreateMap<Model.Entity.Server, ServerModel>();
                q.CreateMap<ServerModel, Model.Entity.Server>();
                q.CreateMap<IpModel, IP>();
                q.CreateMap<IP, IpModel>();
                q.CreateMap<MacAddress, MacAddressModel>();
                q.CreateMap<MacAddressModel, MacAddress>();
            });

            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            //Show the login view
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            IView loginWindow = new LoginWindow(viewModel);
            loginWindow.Show();

            //AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            //RegisterWindow registerWindow = new RegisterWindow(viewModel);
            //registerWindow.Show();
        }
    }
}
