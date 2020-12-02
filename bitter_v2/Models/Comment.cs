using bitter_v2.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bitter_v2.Models
{
    public class Comment : BaseModel
    {
        public string Content { get; set; }
        public string TweetID { get; set; }
        public string CommentID { get; set; }
        public string Likes { get; set; }


        public User User
        {
            get;
            set;
        }

        private bool _UserLikedTweet = false;
        public bool UserLikedTweet
        {
            get
            {
                return _UserLikedTweet;
            }
            set
            {
                _UserLikedTweet = value;
            }
        }
        public string Timestamp { get; set; }



        public virtual async Task<Comment> LoadAsync(string commentID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "comment");
            data.Add("commentid", commentID);
            data.Add("userid", App.User.User.ID);

            CommentID = commentID;
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                var content = value.ToString();
                if (String.IsNullOrEmpty(content))
                {
                    throw new CommentNotExistent(CommentID + " Comment not found");
                }
                else
                {
                    Content = value["content"].ToString();
                    Likes = value["likes"].ToString();
                    TweetID = value["tweetid"].ToString();

                    Timestamp = value["timestamp"].ToString();

                    var tmp2 = value["userlikedcomment"].ToString();
                    UserLikedTweet = tmp2 == "True" ? true : false;

                    var userID = value["userid"].ToString();
                    User = new User();
                    await User.LoadAsync(userID);

                }

            }
            return this;
        }

        public async Task<bool> ToggleLike(UserAuthenticator userAuthenticator)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "put");
            data.Add("type", "likecomment");
            data.Add("username", userAuthenticator.Username);
            data.Add("password", userAuthenticator.Password);
            data.Add("commentid", CommentID);

            UserLikedTweet = !UserLikedTweet;
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);
            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                return value["status"].ToString() == "false" ? false : true;
            }
            return false;
        }

        public async Task<bool> PutComment(UserAuthenticator userAuthenticator, string comment, string tweetid)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "put");
            data.Add("type", "putcomment");
            data.Add("userid", userAuthenticator.User.ID);
            data.Add("password", userAuthenticator.Password);
            data.Add("tweetid", tweetid);
            data.Add("content", comment);
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);
            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                return value["status"].ToString() == "false" ? false : true;
            }
            return false;
        }

    }
}
