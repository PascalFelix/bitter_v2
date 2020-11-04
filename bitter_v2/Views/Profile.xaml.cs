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
    public partial class Profile : ContentPage
    {

        public User User { get; set; }

        public string FollowerCount
        {
            get
            {
                return "Follower (" + User.Follower + ")";
            }
        }
        public string FollowsCount
        {
            get
            {
                return "Follower (" + User.Follows + ")";
            }
        }

        public string TweetsCount
        {
            get
            {
                return "Tweets (" + Tweets.Count + ")";
            }
        }



        protected TweetList TweetList = new TweetList();
        public ObservableCollection<Tweet> Tweets
        {
            get
            {
                return TweetList.Tweets;
            }
            set
            {
                TweetList.Tweets = value;
            }
        }

        public Profile()
        {
            BindingContext = this;
            User = App.User.User;
            InitializeComponent();


            var tmp = TweetList.LoadAsync(User.ID, "0").Result;

        }

        public Profile(User user)
        {
            BindingContext = this;
            User = user;
            InitializeComponent();
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Tweet selectedItem = e.SelectedItem as Tweet;
        }
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Tweet tappedItem = e.Item as Tweet;
        }
    }
}