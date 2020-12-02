using bitter_v2.Models.Exceptions;
using bitter_v2.Views;
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
                    _password = value;
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

        public async Task Login(string username = null, string password = null)
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

            var userID = await Authenticate();
            if (!String.IsNullOrEmpty(userID))
            {
                var tmpUser = new User();
                _loggedUser = await tmpUser.LoadAsync(userID);
                _isLoggedIn = true;
            }
        }

        public async Task<bool> Register(string username, string password, string password2)
        {

            var Helper = new LoginHelper();

            if (String.IsNullOrEmpty(username))
            {
                throw new RegistrationFailed("Benutzername ist leer");

            }
            else
            {
                var userNameTaken = await Helper.IsUsernameTaken(username);
                if (userNameTaken)
                {
                    throw new RegistrationFailed("Benutzername bereits vergeben");
                }
            }

            if (password != password2 && !String.IsNullOrEmpty(password))
            {
                throw new RegistrationFailed("Passwörter sind nicht gleich");
            }

            return await Helper.RegisterUser(username, password);

        }



        private async Task<string> Authenticate()
        {
            var Helper = new LoginHelper();

            await Helper.LoadAsync(Username, Password);

            if (Helper.Type == "1")
            {
                return Helper.UserID;
            }

            return "";
        }

        public void Logout()
        {
            Password = "";
            Remeberme = false;
            HeadAppPage.Logout();
        }

        public class LoginHelper : BaseModel
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

            public async Task<bool> IsUsernameTaken(string username)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("method", "get");
                data.Add("type", "usernametaken");
                data.Add("username", username);
                var task = await base.LoadAsync(data);
                JObject tmp = (JObject)JsonConvert.DeserializeObject(task);

                foreach (var x in tmp)
                {
                    var value = x.Value;
                    return value["taken"].ToString() == "True" ? true : false;

                }
                return true;

            }

            public async Task<bool> RegisterUser(string username, string password)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("method", "put");
                data.Add("type", "registeruser");
                data.Add("username", username);
                data.Add("password", password);
                var task = await base.LoadAsync(data);
                JObject tmp = (JObject)JsonConvert.DeserializeObject(task);


                foreach (var x in tmp)
                {
                    var value = x.Value;
                    return value["status"].ToString() == "True" ? true : false;

                }
                return true;
            }


        }

    }
}
