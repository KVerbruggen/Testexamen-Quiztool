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

        public static string User { get; private set; }
        
        static Main()
        {
            dbManager = new DBManager();
            hashManager = new SHA256CryptoServiceProvider();
            User = String.Empty;
        }

        public static bool Login(string username, string password)
        {
            String hashedPassword = BitConverter.ToString(Main.hashManager.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", string.Empty);
            if (dbManager.Login(username, hashedPassword))
            {
                User = username;
                return true;
            }
            return false;
        }

        public static void Logout()
        {
            User = String.Empty;
        }
    }
}
