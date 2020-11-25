using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class CommentList : BaseListModel
    {

        private string TweetID = "";
        public new ObservableCollection<Comment> Collection = new ObservableCollection<Comment>();

        public CommentList(string tweetID)
        {
            TweetID = tweetID;
        }
        public override async Task<BaseListModel> LoadAsync(string userID, string password, string offset)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "comments");
            data.Add("userid", userID);
            data.Add("offset", offset);
            data.Add("tweetid", TweetID);

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
                    foreach (var y in value["commentids"])
                    {
                        Ids.Add(y.ToString());
                        var comment = new Comment();
                        await comment.LoadAsync(y.ToString());
                        Collection.Add(comment);

                    }

                }

            }
            return this;
        }


    }
}
