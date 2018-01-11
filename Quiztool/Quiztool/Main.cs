using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Quiztool
{
    public static class Main
    {
        public static DBManager dbManager;
        public static SHA256CryptoServiceProvider hashManager;
        
        static Main()
        {
            dbManager = new DBManager();
            hashManager = new SHA256CryptoServiceProvider();
        }
    }
}
