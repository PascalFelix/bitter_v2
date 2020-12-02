using bitter_v2.Models;
using bitter_v2.Models.Interfaces;
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
        public delegate void TweetSelected(Tweet tweet);
        public event TweetSelected OnTweetSelected;

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

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await _tweetList.ResetList(App.User.User.ID, App.User.Password);

                    IsRefreshing = false;
                });
            }
        }



        private ITweetList _tweetList = null;
        public ObservableCollection<Tweet> TweetList
        {
            get
            {
                return _tweetList.GetCollection();
            }
        
        }

        public FeedView(ITweetList tweetList, string userID, string password)
        {
            Init(tweetList, userID, password);
        }
    
        private void Init(ITweetList tweetList, string userID, string password)
        {
            BindingContext = this;
            _tweetList = tweetList;
            _tweetList.LoadNextChunkAsync(userID, password);
            IsRefreshing = true;
            _tweetList.GetCollection().CollectionChanged += Collection_CollectionChanged;
            InitializeComponent();
        }

        public FeedView(ITweetList tweetList)
        {
            this.Init(tweetList, App.User.User.ID, App.User.Password);
            //BindingContext = this;
            //_tweetList = tweetList;
            //_tweetList.LoadNextChunkAsync(App.User.User.ID, App.User.Password);
            //IsRefreshing = true;
            //_tweetList.GetCollection().CollectionChanged += Collection_CollectionChanged;
            //InitializeComponent();
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_tweetList.GetCollection().Count == Models.Feed.OffsetInt)
            {
                IsRefreshing = false;
            }

            if(e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach(var item in e.NewItems)
                {
                    var tweet = item as Tweet;
                    tweet.OnProfileClicked += Tweet_OnProfileClicked;
                }
            }

        }

        private void Tweet_OnProfileClicked(User user)
        {
            Navigation.PushAsync(new Profile(user));
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Tweet selectedItem = e.Item as Tweet;

            OnTweetSelected?.Invoke(selectedItem);
        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            _tweetList.LoadNextChunkOnLastIndex(App.User.User.ID, App.User.Password, e.ItemIndex);

        }


    }
}