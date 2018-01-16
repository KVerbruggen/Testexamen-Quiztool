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
    public partial class TeacherQuestionInfo : Page
    {
        private Question question;

        public TeacherQuestionInfo(Question question)
        {
            this.question = question;
            InitializeComponent();
            cbQuestionType.ItemsSource = typeof(QuestionType).GetEnumNames();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
        }

        private void RefreshAnswers()
        {
            if (question.QuestionTypeEnum == QuestionType.Closed)
            {
                foreach (UIElement e in spMultipleChoice.Children)
                {
                    if (e.GetType() == typeof(StackPanel))
                    {
                        spMultipleChoice.Children.Remove(e);
                    }
                }
                int i = 0;
                foreach (Answer answer in question.Answers)
                {
                    WrapPanel wpAnswer = new WrapPanel();
                    CheckBox cbAnswer = new CheckBox() {
                        Name = "chkAnswer" + i,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10)
                    };
                    TextBox tbAnswer = new TextBox()
                    {
                        Name = "tbAnswer" + i,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10),
                        Text = answer.AnswerText
                    };
                    wpAnswer.Children.Add(cbAnswer);
                    wpAnswer.Children.Add(tbAnswer);
                    spMultipleChoice.Children.Add(wpAnswer);
                    i++;
                }
            }
            else
            {
                tbOpenAnswer.Text = question.Answer;
            }
        }

        private void ProcessOpenAnswers()
        {
            question.Answers.Clear();
            string[] answers = tbOpenAnswer.Text.Split(',');
            int i = 0;
            foreach (string answer in answers)
            {
                question.Answers.Add(new Answer {
                    Question = question,
                    AnswerId = (byte)i,
                    IsCorrect = true,
                    AnswerText = answer
                });
                i++;
            }
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            Main.db.RejectChanges();
            Main.Logout();
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Login());
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            Main.db.RejectChanges(question);
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherTopicInfo(question.Topic));
        }

        private void btDone_Click(object sender, RoutedEventArgs e)
        {
            if (question.QuestionTypeEnum == QuestionType.Open)
            {
                ProcessOpenAnswers();
            }
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherTopicInfo(question.Topic));
        }

        private void cbQuestionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbQuestionType.SelectedItem)
            {
                case "Open vraag":
                    spOpenAnswer.Visibility = Visibility.Visible;
                    divMultipleChoice.Visibility = Visibility.Hidden;
                    spMultipleChoiceControls.Visibility = Visibility.Hidden;
                    break;
                case "Multiple choice:":
                default:
                    ProcessOpenAnswers();
                    spOpenAnswer.Visibility = Visibility.Hidden;
                    divMultipleChoice.Visibility = Visibility.Visible;
                    spMultipleChoiceControls.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void btNewAnswer_Click(object sender, RoutedEventArgs e)
        {
            question.Answers.Add(new Answer() { Question = question, IsCorrect = true });
            RefreshAnswers();
        }

        private void btDeleteAnswer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Main.db.DeleteAnswer(question.Answers.Last());
            }
            catch (InvalidOperationException) { }
            RefreshAnswers();
        }
    }
}
