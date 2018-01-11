﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace Quiztool
{
    public class DBManager
    {
        private QuiztoolEntities1 qte;

        public DBManager()
        {
            qte = new QuiztoolEntities1();
        }

        public bool Login(string username, string hashedPassword)
        {
            return (
                from teacher in qte.Teachers
                where teacher.Name.Equals(username) && teacher.Password.Equals(hashedPassword)
                select teacher
            ).Any();
        }

    }
}
