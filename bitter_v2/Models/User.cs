using bitter_v2.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class User : BaseModel
    {
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

        public virtual async Task<User> LoadAsync(string userID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "user");
            data.Add("id", userID);
            ID = userID;
            var task = await base.LoadAsync(data);
            JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

            foreach (var x in tmp)
            {
                string name = x.Key;
                var value = x.Value;
                var content = value.ToString();
                if (String.IsNullOrEmpty(content))
                {
                    throw new UserNonExistent(userID + " Tweet not found");
                }
                else
                {
                    UserName = value["username"].ToString();
                    Follower = value["follower"].ToString();
                    Follows = value["follows"].ToString();
                    Tweets = value["tweets"].ToString();
                    
                }

            }
            return this;
        }


    }
}
