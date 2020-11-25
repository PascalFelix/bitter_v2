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
    public partial class Feed : ContentPage, INewTweetCloseable
    {
        private FeedView FeedView = null;
        public Feed()
        {
            try
            {
                var FeedObject = new FeedViewModel();
                FeedView = new FeedView(FeedObject);
                BindingContext = this;
                InitializeComponent();
                FeedView.OnTweetSelected += FeedView_OnTweetSelected;
                var Stack = (StackLayout)this.FindByName("StackList");
                Stack.Children.Add(FeedView);
            }
            catch (Exception ex)
            {
                DisplayAlert("Fehler", ex.Message, "OK");
            }
        }

        private void FeedView_OnTweetSelected(Tweet tweet)
        {
            var details = new TweetDetails(tweet, this);
            OpenPopUp(details, 450);
        }

        private View popupView = null;
        private bool DetailsOpen = false;
        private void OpenPopUp(View view, int height)
        {
            if (!DetailsOpen)
            {
                DetailsOpen = true;
                popupView = view;
                var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
                AbsoluteLayout.Children.Add(popupView, new Rectangle(10, 10, AbsoluteLayout.Width - 20, height));
            }
        }


        private void ImageButton_Pressed(object sender, EventArgs e)
        {
            var NewTweet = new NewTweet(this);
            OpenPopUp(NewTweet, 250);
        }

        void INewTweetCloseable.CLoseMe(View view)
        {
            var AbsoluteLayout = (AbsoluteLayout)this.FindByName("TestLayout");
            AbsoluteLayout.Children.Remove(view);
            DetailsOpen = false;
            popupView = null;
        }


    }
}