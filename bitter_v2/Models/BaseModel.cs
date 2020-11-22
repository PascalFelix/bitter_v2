using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models
{
    public class BaseModel: ViewModelBase
    {

        protected ApiHandler ApiHandler = new ApiHandler();


        public virtual async System.Threading.Tasks.Task<string> LoadAsync(Dictionary<string,string> data)
        {
            var test = ApiHandler.ExcecuteAsync(data);

            return await test;
        }


    }
}
