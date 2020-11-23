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
    public partial class Feed : ContentPage, INewTweetCloseable, IRefreshable
    {

        private bool DetailsOpen = false;
        public bitter_v2.Models.Feed FeedObject = new bitter_v2.Models.Feed();

        public ObservableCollection<Tweet> Tweets { get { return FeedObject.Tweets; } set { FeedObject.Tweets = value; } }

        private FeedView FeedView = null;
        public Feed()
        {
            FeedView = new FeedView(this);
            BindingContext = this;
            InitializeComponent();
            FeedView.OnTweetSelected += FeedView_OnTweetSelected;
            FeedView.OnEndScroll += FeedView_OnEndScrollAsync;
            Tweets.CollectionChanged += Tweets_CollectionChanged;
            var Stack = (StackLayout)this.FindByName("StackList");
            Stack.Children.Add(FeedView);

            FeedObject.LoadNextChunkAsync(App.User.Username, App.User.Password);
        }

        private void FeedView_OnTweetSelected(Tweet tweet)
        {
            if (!DetailsOpen)
            {
                var details = new TweetDetails(tweet, this);
                var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
                AbsoluteLayout.Children.Add(details, new Rectangle(10, 10, AbsoluteLayout.Width - 20, 450));
                DetailsOpen = true;
            }
        }

        public async void Refresh()
        {
            Tweets.Clear();
            FeedObject.ResetOffset();
            await FeedObject.LoadNextChunkAsync(App.User.Username, App.User.Password);
        }
        private async void FeedView_OnEndScrollAsync(object sender)
        {
            await FeedObject.LoadNextChunkAsync(App.User.Username, App.User.Password);
        }

        private void Tweets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    FeedView.Tweets.Add((Tweet)item);
                }
            }
            else
            {
                FeedView.Tweets.Clear();
            }
        }



        private NewTweet NewTweet = null;
        private void ImageButton_Pressed(object sender, EventArgs e)
        {

            if (!DetailsOpen)
            {
                NewTweet = new NewTweet(this);
                var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
                AbsoluteLayout.Children.Add(NewTweet, new Rectangle(10, 10, AbsoluteLayout.Width - 20, 200));
                DetailsOpen = true;
            }
            else if(NewTweet != null)
            {
                var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
                AbsoluteLayout.Children.Remove(NewTweet);
                NewTweet = null;
                DetailsOpen = false;
            }

        }

        void INewTweetCloseable.CLoseMe(View view)
        {
            var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
            AbsoluteLayout.Children.Remove(view);
            DetailsOpen = false;
        }


    }
}