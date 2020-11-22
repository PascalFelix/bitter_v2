using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class Feed : BaseModel
    {
        protected int offsetIncrement = 0;
        protected int offset = 0;
        public const int OffsetInt = 10;
        protected List<string> tweetIDs = new List<string>();
        public ObservableCollection<Tweet> Tweets = new ObservableCollection<Tweet>();

        public virtual async Task<Feed> LoadNextChunkAsync(string userID, string password)
        {
            offset += offsetIncrement;
            return await LoadAsync(userID, password, offset.ToString());
        }

        public Feed() : base()
        {
            ResetOffset();
        }


        public void ResetOffset()
        {
            offsetIncrement = Feed.OffsetInt;
            offset = Feed.OffsetInt*-1;
        }

        public virtual async Task<Feed> LoadAsync(string userID, string password, string offset)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "feed");
            data.Add("username", userID);
            data.Add("password", password);
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
                    foreach (var y in value["tweetIDs"])
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
