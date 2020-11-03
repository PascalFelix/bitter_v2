using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models.Exceptions
{
    class TweetNonExistent : Exception
    {
        public TweetNonExistent(string msg) : base(msg)
        {

        }
    }
}
