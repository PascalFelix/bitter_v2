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
    public class User : BaseModel
    {
        public Command FollowCommand { get; set; }


        private string _UserFollowsColor = "Blue";
        private string _UserNotFollowsColor = "Gray";
        private string _CurrentFollowColor = "";

        private string _userName = "";
        public string UserName
        {
            get
            {
                return "@" + _userName;
            }
            set { _userName = value; }
        }
        public string ID { get; set; }
        public string Follower { get; set; }
        public string Follows { get; set; }
        public string Tweets { get; set; }

        public User()
        {
            _CurrentFollowColor = _UserNotFollowsColor;
            FollowCommand = new Command(async x =>
            {
                var TargetUser = x as User;

                await ToogleFollow(TargetUser);
            });
        }


        private bool _CurrentUserFollows = false;

        public bool CurrentUserFollows
        {
            get
            {
                return _CurrentUserFollows;
            }
            set
            {

                if (value)
                {
                    _CurrentFollowColor = _UserFollowsColor;
                }
                else
                {
                    _CurrentFollowColor = _UserNotFollowsColor;
                }
                _CurrentUserFollows = value;
                OnPropertyChanged("GetFollowedBackgroundColor");
            }
        }

        public string GetFollowedBackgroundColor
        {
            get
            {
                return _CurrentFollowColor;
            }
        }

        private bool _hiddenfollowPossible = false;

        private bool _followPossible
        {
            get
            {
                return _hiddenfollowPossible;
            }
            set
            {
                OnPropertyChanged("FollowPossible");
                _hiddenfollowPossible = value;
            }
        }

        public bool FollowPossible
        {
            get
            {
                return _followPossible;
            }
        }



        public async Task<bool> ToogleFollow(User TargetUser)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "put");
            data.Add("type", "togglefollow");
            data.Add("userid", App.User.User.ID);
            data.Add("password", App.User.Password);
            data.Add("targetuserid", TargetUser.ID);

            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);
            CurrentUserFollows = !CurrentUserFollows;
            foreach (var x in tmp)
            {
                var value = x.Value;
                return value["status"].ToString() == "True" ? true : false;
            }
            return false;
        }

        public virtual async Task<User> LoadAsync(string userID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "user");
            data.Add("id", userID);
            if (App.User.User != null)
                data.Add("currentuserid", App.User.User.ID);
            ID = userID;
            if(App.User.User != null && ID != App.User.User.ID)
            {
                _followPossible = true;
            }
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                var content = value.ToString();
                if (String.IsNullOrEmpty(content))
                {
                    throw new UserNonExistent(userID + " User not found");
                }
                else
                {
                    UserName = value["username"].ToString();
                    Follower = value["follower"].ToString();
                    Follows = value["follows"].ToString();
                    Tweets = value["tweets"].ToString();
                    CurrentUserFollows = value["currentuserfollows"].ToString() == "True" ? true : false;

                }

            }
            return this;
        }


    }
}
