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

        public void Save()
        {
            db.SaveChanges();
        }

    }
}
