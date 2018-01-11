using System;
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
        private QuiztoolEntities1 db;

        public DBManager()
        {
            db = new QuiztoolEntities1();
        }

        public bool Login(string username, string hashedPassword)
        {
            return (
                from Teacher in db.Teachers
                where Teacher.Name.Equals(username) && Teacher.Password.Equals(hashedPassword)
                select Teacher
            ).Any();
        }

        public List<Subject> GetSubjects()
        {
            return (
                from Subject in db.Subjects
                select Subject
            ).ToList();
        }

        public void DeleteSubject(Subject subject)
        {
            db.Subjects.Remove(subject);
            db.SaveChanges();
        }

    }
}
