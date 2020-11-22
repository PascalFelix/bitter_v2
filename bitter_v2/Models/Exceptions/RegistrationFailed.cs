using System;
using System.Collections.Generic;
using System.Text;

namespace bitter_v2.Models.Exceptions
{
    class RegistrationFailed : Exception
    {

        public RegistrationFailed(string msg) : base(msg)
        {
           
        }

    }
}
