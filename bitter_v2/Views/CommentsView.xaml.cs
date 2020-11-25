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
    public partial class CommentsView : ContentView
    {

        private Tweet Tweet = null;

        public CommentListViewModel CommentListObject = null;

        public ObservableCollection<Comment> Comments
        {
            get
            {
                return CommentListObject.Collection;
            }
            set
            {
                CommentListObject.Collection = value;
            }
        }

        public CommentsView(Tweet tweet)
        {
            BindingContext = this;
            Tweet = tweet;
            CommentListObject = new CommentListViewModel(Tweet.TweetID);

            InitializeComponent();
            Reload();

        }

        public async void Reload()
        {
            Comments.Clear();
            CommentListObject.ResetOffset();
            await CommentListObject.LoadNextChunkAsync(App.User.User.ID,App.User.Password);
        }
    }
}