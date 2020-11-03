using bitter_v2.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class Tweet : BaseModel
    {

        public string TweetID { get; set; }
        public string Content { get; set; }
        public string Likes { get; set; }
        public string Retweets { get; set; }
        public string UserID { get; set; }

        public User User { get; set; }

        public Tweet()
        {
            User = new User();
        }

        public virtual async Task<Tweet> LoadAsync(string tweetID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "tweet");
            data.Add("id", tweetID);
            TweetID = tweetID;
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                var content = value.ToString();
                if (String.IsNullOrEmpty(content))
                {
                    throw new TweetNonExistent(TweetID + " Tweet not found");
                }
                else
                {
                    Content = value["content"].ToString();
                    Likes = value["likes"].ToString();
                    Retweets = value["retweets"].ToString();
                    UserID = value["userid"].ToString();
                    await User.LoadAsync(UserID);
                }

            }
            return this;
        }

    }
}
