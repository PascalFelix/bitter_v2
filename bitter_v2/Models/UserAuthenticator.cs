﻿using bitter_v2.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace bitter_v2.Models
{
    public class UserAuthenticator
    {

        private User _loggedUser = null;
        public User User { get { return _loggedUser; } }

        public App HeadAppPage = null;


        public bool Remeberme
        {
            get
            {
                return Preferences.Get("remember_me", false);
            }
            set
            {
                Preferences.Set("remember_me", value);
            }
        }

        public string Username
        {
            get
            {
                return Preferences.Get("username", "");
            }
            set
            {
                Preferences.Set("username", value);
            }
        }

        private string _password = "";
        public string Password
        {
            get
            {
                if (Remeberme)
                {
                    return Preferences.Get("password", "");
                }
                else
                {
                    return _password;
                }
            }
            set
            {
                if (Remeberme)
                {
                    Preferences.Set("password", value);
                }
                else
                {
                    _password = value;
                }
            }
        }



        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
        }

        public UserAuthenticator(App app)
        {
            HeadAppPage = app;
        }

        public void Login(string username = null, string password = null)
        {
            //TODO IMPLEMENT LOGIN USER ALWAYS 1
            if (!String.IsNullOrEmpty(username))
            {
                Username = username;
            }

            if (!String.IsNullOrEmpty(password))
            {
                Password = password;
            }

            var userID = Authenticate();
            if (!String.IsNullOrEmpty(userID))
            {
                var tmpUser = new User();
                _loggedUser = tmpUser.LoadAsync(userID).Result;
                _isLoggedIn = true;
            }

        }



        private string Authenticate()
        {
            var Helper = new LoginHelper();

            if(Helper.LoadAsync(Username,Password).Result.Type == "1")
            {
                return Helper.UserID;
            }

            return null;
        }

        public void Logout()
        {
            Password = "";
            Remeberme = false;
            HeadAppPage.Logout();
        }

        class LoginHelper : BaseModel
        {

            public string Type { get; set; }
            public string UserID { get; set; }

            public virtual async Task<LoginHelper> LoadAsync(string username, string password)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("method", "get");
                data.Add("type", "login");
                data.Add("username", username);
                data.Add("password", password);


                var task = await base.LoadAsync(data);
                JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

                foreach (var x in tmp)
                {
                    string name = x.Key;
                    var value = x.Value;
                    var content = value.ToString();
                    Type = value["type"].ToString();
                    UserID = value["userid"].ToString();
                }
                return this;
            }

        }

    }
}
