using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class TweetList : BaseModel
    {
        protected int offset = 0;
        protected int offsetIncrement = 5;
        protected List<string> tweetIDs = new List<string>();
        public ObservableCollection<Tweet> Tweets = new ObservableCollection<Tweet>();

        public virtual async Task<TweetList> LoadAsync(string userID, string offset)
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
                    //tweetIDs = JsonConvert.DeserializeObject(value["tweetIds"]);
                    foreach(var y in value["tweetIds"])
                    {
                        tweetIDs.Add(y.ToString());
                        var tweet = new Tweet();
                        await tweet.LoadAsync(y.ToString());
                        Tweets.Add(tweet);

                    }
         
                    
                }

            }
            return this;
        }
    }
}
