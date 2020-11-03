using bitter_v2.Models;
using bitter_v2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2
{
    public partial class App : Application
    {

        public static UserAuthenticator User = null;

        public App()
        {
            User = new UserAuthenticator(this);
          
            InitializeComponent();
            Relogin();
        }

        public void Relogin()
        {
            User.Login();
            if (User.IsLoggedIn)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new Login(this);
            }
        }
        public void Logout()
        {
            MainPage = new Login(this);
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
