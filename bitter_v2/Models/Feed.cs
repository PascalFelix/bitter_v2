﻿using bitter_v2.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace bitter_v2.Models
{
    public class Feed : BaseListModel, ITweetList
    {

        public ObservableCollection<Tweet> Collection = new ObservableCollection<Tweet>();

        public Feed() : base()
        {

        }

        public override void ResetList()
        {
            base.ResetList();
            this.Collection.Clear();
        }

        public new ObservableCollection<Tweet> GetCollection()
        {
            return this.Collection;
        }

        public override async Task<BaseListModel> LoadAsync(string userID, string password, string offset)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("method", "get");
            data.Add("type", "feed");
            data.Add("userid", userID);
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
                    foreach (var y in value["tweetIDs"])
                    {
                        Ids.Add(y.ToString());
                        var tweet = new Tweet(userID, password);
                        await tweet.LoadAsync(y.ToString());
                        this.Collection.Add(tweet);
                    }
                    _firstLoadDone = true;
                }
            }
            return this;
        }
    }
}
