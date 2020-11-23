using bitter_v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserListView : ContentView
    {


        private Models.UserList _UserList = null;

        public ObservableCollection<User> UserList
        {
            get
            {
                return _UserList.Users;
            }
            set
            {
                _UserList.Users = value;
            }
        }

        public UserListView()
        {
            BindingContext = this;
            _UserList = new Models.UserList();
            InitializeComponent();
            _UserList.LoadAsync();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}