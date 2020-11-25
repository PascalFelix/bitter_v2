using bitter_v2.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class TweetList : BaseListModel, ITweetList
    {

        public ObservableCollection<Tweet> Collection = new ObservableCollection<Tweet>();
        public override void ResetList()
        {
            base.ResetList();
            this.Collection.Clear();
        }

        public new ObservableCollection<Tweet> GetCollection()
        {
            return this.Collection;
        }
        public override async Task<BaseListModel> LoadAsync(string userID, string password, string offset)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "tweets");
            data.Add("id", userID);
            data.Add("offset", offset);

            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                var content = value.ToString();
                if (String.IsNullOrEmpty(content))
                {
                    throw new Exception("error");
                }
                else
                {
                    foreach (var y in value["tweetIds"])
                    {
                        Ids.Add(y.ToString());
                        var tweet = new Tweet(App.User.User.ID, App.User.Password);
                        await tweet.LoadAsync(y.ToString());
                        GetCollection().Add(tweet);
                    }
                }
            }
            return this;
        }
    }
}
