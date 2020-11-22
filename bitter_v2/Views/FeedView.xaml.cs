using bitter_v2.Models;
using bitter_v2.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedView : ContentView
    {

        public delegate void StatusUpdateHandler(object sender);
        public event StatusUpdateHandler OnEndScroll;

        public delegate void TweetSelected(Tweet tweet);
        public event TweetSelected OnTweetSelected;

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

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public IRefreshable refreshable = null;

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    refreshable.Refresh();

                    IsRefreshing = false;
                });
            }
        }

        private bool _isFirstLoad = true;
        private bool isFristLoad
        {
            get
            {
                return _isFirstLoad;
            }
            set
            {
                _isFirstLoad = value;
            }
        }

        //public Command ReloadButtonClicked { get; set; }
        public FeedView(IRefreshable _refreshable)
        {
            BindingContext = this;
            refreshable = _refreshable;
            IsRefreshing = true;
            InitializeComponent();
            Tweets.CollectionChanged += Tweets_CollectionChanged;

        }
        private void Tweets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(Tweets.Count == Models.Feed.OffsetInt && isFristLoad)
            {
                isFristLoad = false;
                IsRefreshing = false;
            }
        }


        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Tweet selectedItem = e.Item as Tweet;
            OnTweetSelected(selectedItem);
        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if(e.ItemIndex == TweetList.Tweets.Count - 1 && !IsRefreshing)
            {
                IsRefreshing = true;
                OnEndScroll(this);
                IsRefreshing = false;
            }

        }





    }
}