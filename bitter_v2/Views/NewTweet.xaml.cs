using bitter_v2.Models;
using bitter_v2.Views.Interfaces;
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
    public partial class NewTweet : ContentView
    {
        private INewTweetCloseable parentView = null;

        public string NewTweetString { get; set; }

        public NewTweet(INewTweetCloseable parent)
        {
            InitializeComponent();
            parentView = parent;
            BindingContext = this;
        }

        private void CommitTweetButton(object sender, EventArgs e)
        {
            var tweet = new Tweet(App.User.User.ID,App.User.Password);
            tweet.PutTweet(App.User.User.ID,App.User.Password, NewTweetString);

            parentView.CLoseMe(this);
        }

        private void ExitButtonClicked(object sender, EventArgs e)
        {
            parentView.CLoseMe(this);
        }

        

    }
}