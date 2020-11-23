using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    class UserList : BaseModel
    {
        protected List<string> userIDs = new List<string>();
        public ObservableCollection<User> Users = new ObservableCollection<User>();
        public virtual async Task<UserList> LoadAsync()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "userlist");

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
                    foreach (var y in value["userids"])
                    {
                        userIDs.Add(y.ToString());
                        var User = new User();
                        await User.LoadAsync(y.ToString());
                        Users.Add(User);
                    }


                }
            }
            return this;
        }
    }
}
