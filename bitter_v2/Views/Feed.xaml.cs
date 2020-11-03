using bitter_v2.Models;
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
    public partial class Feed : ContentPage
    {
        public Feed()
        {
            InitializeComponent();
            ApiHandler test = new ApiHandler();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("test", "test");
            var task = test.ExcecuteAsync(data);

            Hallo.Text = task.Result;

            Tweet  tmp = new Tweet();
            var task2 = tmp.LoadAsync("-1");
            task2.ContinueWith(Test =>
            {
                if(Test.Exception == null)
                {
                    Hallo.Text = tmp.Content;
                }
              
            });

            //Hallo.Text = task2.Result;

        }
    }
}