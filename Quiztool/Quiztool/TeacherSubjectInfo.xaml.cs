using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quiztool
{
    /// <summary>
    /// Interaction logic for TeacherSubjectInfo.xaml
    /// </summary>
    public partial class TeacherSubjectInfo : Page
    {
        private Subject subject;
        private Action popupAction;

        public TeacherSubjectInfo(Subject subject)
        {
            if (subject == null)
            {
                this.subject = subject;
            }
            else
            {
                this.subject = new Subject();
            }
            // GetExams();
            // GetTopics();
            InitializeComponent();
            this.tbName.Text = subject.Name;
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
            lbExams.ItemsSource = subject.Exams;
            lbExams.SelectedIndex = 0;
            lbTopics.ItemsSource = subject.Topics;
            lbTopics.SelectedIndex = 0;
            popupAction = GoBack;
        }

        /*
         * Deze functies zijn overbodig.
        private void GetTopics()
        {
            if (subject != null)
            {
                foreach (Topic topic in subject.Topics)
                {
                    lbTopics.Items.Add(topic);
                }
                lbTopics.SelectedIndex = 0;
            }
        }

        private void GetExams()
        {
            List<Exam> exams = new List<Exam>();
            if (subject != null)
            {
                foreach (Exam exam in subject.Exams)
                {
                    exams.Add(exam);
                }
                lbExams.SelectedIndex = 0;
            }
        }
        */

        private void GoBack()
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectList());
        }

        private void Logout()
        {
            Main.Logout();
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Login());
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            if (Main.db.HasUnsavedChanges)
            {
                popupAction = Logout;
                popupNotSaved.IsOpen = true;
            }
            else
            {
                Logout();
            }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (Main.db.HasUnsavedChanges)
            {
                popupAction = GoBack;
                popupNotSaved.IsOpen = true;
            }
            else
            {
                GoBack();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            Main.db.Save();
        }

        private void btPopupNotSavedYes_Click(object sender, RoutedEventArgs e)
        {
            Main.db.Save();
            popupAction();
        }

        private void btPopupNotSavedNo_Click(object sender, RoutedEventArgs e)
        {
            Main.db.RejectChanges();
            popupAction();
        }

        private void btPopupNotSavedCancel_Click(object sender, RoutedEventArgs e)
        {
            popupNotSaved.IsOpen = false;
        }

        private void lbTopics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblTopicName.Content = ((Topic)lbTopics.SelectedItem).Name;
            lblTopicNQuestions.Content = "Aantal vragen: " + ((Topic)lbTopics.SelectedItem).Questions.Count;
            spTopic.Children.Clear();
            foreach (Question question in ((Topic)lbTopics.SelectedItem).Questions)
            {
                spTopic.Children.Add(new Label() {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Content = question.QuestionText,
                    Margin = new Thickness(0, 0, 0, -10),
                });
            }
        }

        private void lbExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Exam)lbExams.SelectedItem != null) {
                Exam exam = ((Exam)lbExams.SelectedItem);
                lblExamName.Content = exam.Name;
                lblExamTopics.Content = "Onderwerpen: " + exam.GetTopicsAsString();
                lblExamNQuestions.Content = "Aantal vragen: " + exam.NQuestions;

                string maxScore = (exam.GetMaxScore() == 0) ? "-" : exam.GetMaxScore().ToString();
                lblExamMaxScore.Content = "Totale score: " + maxScore;
                lblExamMinScore.Content = "Minimumscore: " + exam.Minimumscore;

                string minimumTopicsPassed = ((int)exam.MinimumTopicsPassed == 0) ? "-" : ((int)exam.MinimumTopicsPassed).ToString();
                lblMinTopics.Content = "Minimum aantal onderwerpen: " + minimumTopicsPassed;

                string timelimit = (exam.Timelimit == 0) ? "-" : exam.Timelimit.ToString();
                lblTimeLimit.Content = "Tijdslimiet: " + timelimit;
            }
        }

        private void btDeleteTopic_Click(object sender, RoutedEventArgs e)
        {
            Main.db.DeleteTopic((Topic)lbTopics.SelectedItem);
        }

        private void btEditTopic_Click(object sender, RoutedEventArgs e)
        {
            if ((Topic)lbTopics.SelectedItem != null)
            {
                ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherTopicInfo((Topic)lbTopics.SelectedItem));
            }
        }

        private void btNewTopic_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherTopicInfo(new Topic() { Subject = subject }));
        }

        private void btDeleteExam_Click(object sender, RoutedEventArgs e)
        {
            Main.db.DeleteExam((Exam)lbExams.SelectedItem);
        }

        private void btEditExam_Click(object sender, RoutedEventArgs e)
        {
            if ((Exam)lbExams.SelectedItem != null){
                ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherExamInfo((Exam)lbExams.SelectedItem));
            }
        }

        private void btNewExam_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherExamInfo(new Exam() { Subject = subject }));
        }

    }
}
