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
    public partial class TweetDetails : ContentView
    {
        private Tweet _tweet = null;
        private INewTweetCloseable tweetCloseable = null;
        public Tweet Tweet { get { return _tweet; } set { _tweet = value; } }

        public TweetDetails(Tweet CurrentTweet, INewTweetCloseable newTweetCloseable)
        {
            BindingContext = this;
            _tweet = CurrentTweet;
            tweetCloseable = newTweetCloseable;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            tweetCloseable.CLoseMe(this);
        }
    }
}