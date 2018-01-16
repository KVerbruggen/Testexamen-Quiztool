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

        public TeacherSubjectInfo(Subject subject = null)
        {
            this.subject = subject;
            GetExams();
            GetTopics();
            InitializeComponent();
            this.tbName.Text = subject.Name;
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
            popupAction = GoBack;
        }

        private void GetTopics()
        {
            // TO-DO: Find out if this is valid, or if a separate query is necessary (function for this already created in DBManager as GetTopics(Subject subject)
            // TO-DO: Same for GetExams(), lbTopics_SelectionChanged() and lbExams_SelectionChanged
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
            if (subject != null)
            {
                foreach (Exam exam in subject.Exams)
                {
                    lbExams.Items.Add(exam);
                }
                lbExams.SelectedIndex = 0;
            }
        }

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
            lblTopicNQuestions.Content = ((Topic)lbTopics.SelectedItem).Questions.Count;
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
            lblExamName.Content = ((Topic)lbExams.SelectedItem).Name;
            lblTopicNQuestions.Content = ((Topic)lbTopics.SelectedItem).Questions.Count;
            spExam.Children.Clear();
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
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherExamInfo(subject));
        }

    }
}
