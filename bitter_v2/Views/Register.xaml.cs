using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public string RegisterUserName
        {
            get;
            set;
        }
        public string RegisterPassword
        {
            get;
            set;
        }
        public string RegisterPassword2
        {
            get;
            set;
        }

        private App HeadApp = null;

        public bool Remeberme
        {
            get
            {
                return App.User.Remeberme;
            }
            set
            {
                App.User.Remeberme = value;
            }
        }
        public Register(App app)
        {
            BindingContext = this;
            HeadApp = app;
            InitializeComponent();
        }
        private async void RegisterBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var returnValue = await App.User.Register(RegisterUserName, RegisterPassword, RegisterPassword2);
                if (returnValue)
                {
                    App.User.Username = RegisterUserName;
                    App.User.Password = RegisterPassword;
                    Remeberme = false;
                    HeadApp.Relogin();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", ex.Message, "Okay");
            }
        }
    }
}