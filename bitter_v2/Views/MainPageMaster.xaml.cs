using bitter_v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;


        public MainPageMaster()
        {
          
            InitializeComponent();
            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            App.User.Logout();
            App.User = new UserAuthenticator(App.app);
        }
        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public User LoggedUser { get; set; }
            public ObservableCollection<MainPageMasterMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                LoggedUser = App.User.User;
                MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
                {
                    new MainPageMasterMenuItem { Id = 0, Title = "Feed",TargetType = typeof(Feed) },
                    new MainPageMasterMenuItem { Id = 1, Title = "Profil",TargetType = typeof(Profile) },
                    new MainPageMasterMenuItem { Id = 2, Title = "Alle Benutzer",TargetType = typeof(UserList) },

                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}