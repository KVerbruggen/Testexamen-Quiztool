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

        public bool HasUnsavedChanges {
            get { return db.ChangeTracker.HasChanges(); }
        }

        public DBManager()
        {
            db = new QuiztoolEntities1();
        }

        public bool Login(string username, string hashedPassword)
        {
            return db.Teachers.Where(t => t.Name.Equals(username) && t.Password.Equals(hashedPassword)).Any();
        }

        public List<Subject> GetSubjects()
        {
            return db.Subjects.ToList();
        }

        /*
         * These functions are unnecessary or invalid in a code-first implementation.
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

        public List<Topic> GetTopics(Subject subject)
        {
            return (
                from Topic in db.Topics
                where Topic.Subject.Equals(subject)
                select Topic
                ).ToList();
        }

        public List<Topic> GetTopics(Exam exam)
        {
            return (
                from ExamTopic in db.ExamTopics
                where ExamTopic.Exam.Equals(exam)
                select ExamTopic.Topic
                ).ToList();
        }

        public int GetQuestionCount(Topic topic)
        {
            return (
                from Question in db.Questions
                where Question.Topic.Equals(topic)
                select Question
            ).Count();
        }

        public List<Question> GetQuestions(Topic topic)
        {
            return (
                from Question in db.Questions
                where Question.Topic.Equals(topic)
                select Question
            ).ToList();
        }

        public List<Exam> GetExams(Subject subject)
        {
            return (
                from Exam in db.Exams
                where Exam.Subject.Equals(subject)
                select Exam
                ).ToList();
        }
        */

        public void DeleteSubject(Subject subject)
        {
            db.Subjects.Remove(subject);
        }

        public void DeleteTopic(Topic topic)
        {
            db.Topics.Remove(topic);
        }

        public void DeleteExam(Exam exam)
        {
            db.Exams.Remove(exam);
        }

        public void DeleteAnswer(Answer answer)
        {
            db.Answers.Remove(answer);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void RejectChanges(DbEntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }

        public void RejectChanges(Topic topic)
        {
            foreach (DbEntityEntry entry in db.ChangeTracker.Entries())
            {
                if (entry.Entity.Equals(topic) || topic.Questions.Contains(entry.Entity))
                {
                    RejectChanges(entry);
                }
            }
        }

        public void RejectChanges(Exam exam)
        {
            foreach (DbEntityEntry entry in db.ChangeTracker.Entries())
            {
                if (entry.Entity.Equals(exam))
                {
                    RejectChanges(entry);
                }
            }
        }

        public void RejectChanges(Question question)
        {
            foreach (DbEntityEntry entry in db.ChangeTracker.Entries())
            {
                if (entry.Entity.GetType() == typeof(Answer))
                {
                    if (((Answer)(entry.Entity)).Question.Equals(question))
                    {
                        RejectChanges(entry);
                    }
                }
                else if (entry.Entity.Equals(question))
                {
                    RejectChanges(entry);
                }
            }
        }

        public void RejectChanges()
        {
            foreach (DbEntityEntry entry in db.ChangeTracker.Entries())
            {
                RejectChanges(entry);
            }
        }
    }
}
