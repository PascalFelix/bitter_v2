using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private App HeadApp = null;

        public string UserName
        {
            get
            {
                return App.User.Username;
            }
            set
            {
                App.User.Username = value;
            }
        }
        public string Password
        {
            get
            {
                return App.User.Password;
            }
            set
            {
                App.User.Password = value;
            }
        }
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
        public ICommand RegisterCommand => new Command(OpenRegister);
        public Login(App app)
        {
            BindingContext = this;
            HeadApp = app;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.User.Username = UserName;
            App.User.Password = Password;
            HeadApp.Relogin();
        }
        private void OpenRegister()
        {
            var reg = new Register(HeadApp);
            reg.Title = "Regrestrieren";
            Navigation.PushAsync(reg);
        }


    }
}