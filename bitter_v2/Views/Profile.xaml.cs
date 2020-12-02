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
    public partial class Profile : ContentPage,INewTweetCloseable
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
                return "Follows (" + User.Follows + ")";
            }
        }

        public string TweetsCount
        {
            get
            {
                return "Tweets (" + User.Tweets + ")";
            }
        }

        private FeedView FeedView = null;

        public Profile()
        {
            BindingContext = this;
            User = App.User.User;
            InitializeComponent();
            Init(App.User.User, App.User.User.ID,App.User.Password);

        }

        private void Init(User user, string UserID, string password = "")
        {
            var tmp = new TweetList();

         
            FeedView = new FeedView(tmp, UserID,password);
   
            var Grid = (Grid)this.FindByName("ProfileGrid");
            FeedView.IsClippedToBounds = true;
            Grid.Children.Add(FeedView);
            Grid.SetColumn(FeedView, 0);
            Grid.SetRow(FeedView, 1);
            Grid.SetColumnSpan(FeedView, 3);
            FeedView.OnTweetSelected += FeedView_OnTweetSelected;
        }

        private void FeedView_OnTweetSelected(Tweet tweet)
        {
            var details = new TweetDetails(tweet, this);
            OpenPopUp(details, 450);
        }
    

        public Profile(User user)
        {
            BindingContext = this;
            User = user;
            InitializeComponent();
            Init(User, User.ID);

        }
    
        private View popupView = null;
        private bool DetailsOpen = false;
        private void OpenPopUp(View view, int height)
        {
            if (!DetailsOpen)
            {
                DetailsOpen = true;
                popupView = view;
                var AbsoluteLayout = (AbsoluteLayout)this.FindByName("AbsolutLayoutProfile");
                AbsoluteLayout.Children.Add(popupView, new Rectangle(10, 10, AbsoluteLayout.Width - 20, height));
            }
        }

        void INewTweetCloseable.CLoseMe(View view)
        {
            var AbsoluteLayout = (AbsoluteLayout)this.FindByName("AbsolutLayoutProfile");
            AbsoluteLayout.Children.Remove(view);
            DetailsOpen = false;
            popupView = null;
        }

    }
}