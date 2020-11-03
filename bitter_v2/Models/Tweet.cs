using bitter_v2.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    class Tweet : BaseModel
    {

        protected string TweetID = "";
        public string Content = "";
        protected string Likes = "";
        protected string Retweets = "";

        public virtual async Task<bool> LoadAsync(string tweetID)
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
                }

            }
            return true;
        }

    }
}
