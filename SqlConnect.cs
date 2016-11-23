using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class SqlConnect
    {
        public static string SqlString()
        {
            return "server=LOCALHOST;database=DBLibrary;Trusted_Connection=yes";
        }
    }
}