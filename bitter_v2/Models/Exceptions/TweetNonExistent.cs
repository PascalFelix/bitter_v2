﻿using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models.Exceptions
{
    public class TweetNonExistent : Exception
    {
        public TweetNonExistent(string msg) : base(msg)
        {

        }
    }
}
