using bitter_v2.Models;
using bitter_v2.Views.Interfaces;
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
    public partial class TweetDetails : ContentView
    {
        private Tweet _tweet = null;
        private INewTweetCloseable tweetCloseable = null;
        public Tweet Tweet { get { return _tweet; } set { _tweet = value; } }

        private CommentsView CommentsView = null;

        public string NewCommentString
        {
            get; set;
        }
        public ICommand ToggleNewComment => new Command(NewTweetToogle);

        public TweetDetails(Tweet CurrentTweet, INewTweetCloseable newTweetCloseable)
        {
            BindingContext = this;
            _tweet = CurrentTweet;
            tweetCloseable = newTweetCloseable;
            InitializeComponent();
            CommentsView = new CommentsView(Tweet);
            var CommentStackPlaceholder = (StackLayout)this.FindByName("CommentStackPlaceholder");
            CommentStackPlaceholder.Children.Add(CommentsView);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            tweetCloseable.CLoseMe(this);
        }
        private async void NewCommentCommit(object sender, EventArgs e)
        {
            var newComment = new Comment();
            await newComment.PutComment(App.User, NewCommentString, Tweet.TweetID);
            NewTweetToogle();
            CommentsView.Reload();
        }

        private void NewTweetToogle()
        {
            var btn = (StackLayout)this.FindByName("newCommentStack");
            btn.IsVisible = !btn.IsVisible;
        }


    }
}