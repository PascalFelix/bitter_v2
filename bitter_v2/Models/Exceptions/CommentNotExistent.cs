using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models.Exceptions
{
    class CommentNotExistent : Exception
    {

        public CommentNotExistent(string msg) : base(msg)
        {

        }

    }
}
