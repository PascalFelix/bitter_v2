using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models
{
    public class BaseModel
    {

        protected ApiHandler ApiHandler = new ApiHandler();


        public virtual async System.Threading.Tasks.Task<string> LoadAsync(Dictionary<string,string> data)
        {
            return await ApiHandler.ExcecuteAsync(data);
        }


    }
}
