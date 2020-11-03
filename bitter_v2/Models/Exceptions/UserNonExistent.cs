using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models.Exceptions
{
    public class UserNonExistent : Exception
    {

        public UserNonExistent(string msg) : base(msg)
        {



        }
    }
}
