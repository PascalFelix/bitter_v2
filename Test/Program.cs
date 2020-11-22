using bitter_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bitter_v2.Models.UserAuthenticator;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("start");
            TestAsync();
            Console.WriteLine("after");
            var tmp = Console.ReadLine();
        }

        static async void TestAsync()
        {
            var test = new LoginHelper();
           
            await test.LoadAsync("Test","123");
     
            Console.WriteLine(test.Type.ToString());
        }
    }
}
