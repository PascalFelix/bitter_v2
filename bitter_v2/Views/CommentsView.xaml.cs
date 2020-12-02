using bitter_v2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bitter_v2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsView : ContentView
    {

        private Tweet Tweet = null;

        public CommentListViewModel CommentListObject = null;
        public ICommand OpenProfileCommand
        {
            get
            {
                return new Command((comment) =>
                {
                    Comment tmp = comment as Comment;
                    Navigation.PushAsync(new Profile(tmp.User));
                });
            }
        }
        public ObservableCollection<CommentViewModel> Comments
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
            Comments.CollectionChanged += Comments_CollectionChanged;
        }

        private void Comments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach(var item in e.NewItems)
                {
                    CommentViewModel tmp = item as CommentViewModel;
                    tmp.OnProfileClicked += Tmp_OnProfileClicked;
                }
                
            }
        }

        private void Tmp_OnProfileClicked(User user)
        {
            Navigation.PushAsync(new Profile(user));
        }

        public async void Reload()
        {
            Comments.Clear();
            CommentListObject.ResetOffset();
            await CommentListObject.LoadNextChunkAsync(App.User.User.ID,App.User.Password);
        }
    }
}