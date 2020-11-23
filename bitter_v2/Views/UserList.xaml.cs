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
    public partial class UserList : ContentPage
    {

        private UserListView UserListView = null;

        public UserList()
        {
            UserListView = new UserListView();
            InitializeComponent();
            var Stack = (StackLayout)this.FindByName("StackList");
            Stack.Children.Add(UserListView);
        }
    }
}