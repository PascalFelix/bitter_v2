using bitter_v2.Models;
using bitter_v2.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage, IRefreshable
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
                return "Tweets (" + User.Tweets + ")";
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

        private FeedView FeedView = null;

        public Profile()
        {
            BindingContext = this;
            FeedView = new FeedView(this);
            User = App.User.User;
            InitializeComponent();
            FeedView.OnEndScroll += FeedView_OnEndScroll;
            Tweets.CollectionChanged += Tweets_CollectionChanged;

            TweetList.LoadAsync(User.ID, "0");

            var Grid = (Grid)this.FindByName("ProfileGrid");
            FeedView.IsClippedToBounds = true;
            Grid.Children.Add(FeedView);
            Grid.SetColumn(FeedView, 0);
            Grid.SetRow(FeedView, 1);
            Grid.SetColumnSpan(FeedView, 3);
        }

        public async void Refresh()
        {

            Tweets = new ObservableCollection<Tweet>();
            await TweetList.LoadAsync(User.ID, "0");

        }

        private void FeedView_OnEndScroll(object sender)
        {

        }

        private void Tweets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                FeedView.Tweets.Add((Tweet)item);
            }
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