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
    public partial class Feed : ContentPage
    {


        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public ObservableCollection<Tweet> Tweets { get { return _tweets; } set { _tweets = value; } }


        public Feed()
        {
            BindingContext = this;
            InitializeComponent();
            //ApiHandler test = new ApiHandler();
            //Dictionary<string, string> data = new Dictionary<string, string>();
            //data.Add("test", "test");
            //var task = test.ExcecuteAsync(data);

            //Hallo.Text = task.Result;




            Tweet tmp = new Tweet();
            var task2 = tmp.LoadAsync("26");
            task2.ContinueWith(Test =>
            {
                if (Test.Exception == null)
                {
                    var teweet = Test.Result;
                    Tweets.Add(teweet);
                }

            });
            Tweet tmp2 = new Tweet();
            var task22 = tmp.LoadAsync("24");
            task2.ContinueWith(Test =>
            {
                if (Test.Exception == null)
                {
                    var teweet = Test.Result;
                    Tweets.Add(teweet);
                }

            });
            Tweets.CollectionChanged += Tweets_CollectionChanged;

        }

        private void Tweets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
          
        }
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Tweet selectedItem = e.SelectedItem as Tweet;
        }
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Tweet tappedItem = e.Item as Tweet;
        }
    }
}