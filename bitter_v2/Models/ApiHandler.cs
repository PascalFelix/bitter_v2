using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace bitter_v2.Models
{
    public class ApiHandler
    {

        private static readonly HttpClient client = new HttpClient();
        private static bool CallbackSet = false;


        protected string ApiUrl = "http://hshl-bitter.de/";


        public ApiHandler()
        {
            if (!CallbackSet)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
        }

        public async System.Threading.Tasks.Task<string> ExcecuteAsync(Dictionary<string, string> data)
        {
            string output = JsonConvert.SerializeObject(data);
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(ApiUrl, content);
            //var result = response.Result;
            return await response.Content.ReadAsStringAsync();
        }

    }
}
