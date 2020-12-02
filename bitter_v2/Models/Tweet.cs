using bitter_v2.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bitter_v2.Models
{
    public class Tweet : BaseModel, INotifyPropertyChanged
    {

        private string NotLikedImageSrc = "noLike.img";
        private string LikedImageSrc = "like.img";

        public delegate void ProfileClickedDelegate(User user);
        public event ProfileClickedDelegate OnProfileClicked;

        public Command ReloadButtonClicked { get; set; }

        public Command ProfileClicked
        {
            get
            {
                return new Command((x) =>
                {
                    OnProfileClicked?.Invoke(this.User);
                });
            }
        }

        public string TweetID { get; set; }
        public string Content { get; set; }
        public string Likes { get; set; }
        public string Retweets { get; set; }
        public string UserID { get; set; }
        public string Timestamp { get; set; }



        private bool _UserLikedTweet = false;
        public bool UserLikedTweet
        {
            get
            {
                return _UserLikedTweet;
            }
            set
            {
                if (value)
                {
                    GetLikeImgSrc = LikedImageSrc;
                }
                else
                {
                    GetLikeImgSrc = NotLikedImageSrc;
                }
                OnPropertyChanged("GetLikeImgSrc");
                _UserLikedTweet = value;
            }
        }

        private string _GetLikeImgSrc = "";
        public string GetLikeImgSrc
        {
            get
            {
                return _GetLikeImgSrc;
            }
            set
            {
                _GetLikeImgSrc = value;
            }
        }


        public User User { get; set; }

        public Tweet(string userid,string password)
        {
            _GetLikeImgSrc = LikedImageSrc;
            User = new User();

            ReloadButtonClicked = new Command(async x =>
        {
            await ToggleLike(userid, password);
        });
        }

        public virtual async Task<Tweet> LoadAsync(string tweetID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "tweet");
            data.Add("id", tweetID);

            data.Add("userid", App.User.User.ID);
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
                    Timestamp = value["timestamp"].ToString();
                    var tmp2 = value["userlikedtweet"].ToString();
                    UserLikedTweet = tmp2 == "True" ? true : false;
                    await User.LoadAsync(UserID);
                }

            }
            return this;
        }

        public async Task<bool> PutTweet(string userid,string password, string tweet)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "put");
            data.Add("type", "tweet");
            data.Add("userid", userid);
            data.Add("password", password);
            data.Add("tweet", tweet);
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

        public async Task<bool> ToggleLike(string userid, string password)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "put");
            data.Add("type", "like");
            data.Add("userid", userid);
            data.Add("password", password);
            data.Add("tweetid", TweetID);
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

    }
}
