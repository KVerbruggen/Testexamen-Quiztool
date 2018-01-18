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
            if (question.QuestionTypeEnum == QuestionType.Open)
            {
                cbQuestionType.SelectedIndex = 0;
            }
            else
            {
                cbQuestionType.SelectedIndex = 1;
            }
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
            RefreshAnswers();
        }

        private void RefreshAnswers()
        {
            if (question.QuestionTypeEnum == QuestionType.Closed)
            {
                spMultipleChoice.Children.Clear();
                int i = 0;
                foreach (Answer answer in question.Answers)
                {
                    WrapPanel wpAnswer = new WrapPanel();
                    CheckBox cbAnswer = new CheckBox() {
                        Name = "chkAnswer" + i,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10)
                    };
                    string answerText = "";
                    if (answer.AnswerText == String.Empty)
                    {
                        answerText = "Antwoord " + (i+1);
                    }
                    else
                    {
                        answerText = answer.AnswerText;
                    }
                    TextBox tbAnswer = new TextBox()
                    {
                        Name = "tbAnswer" + i,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Width = Double.NaN, // Width 'auto' is Double.NaN
                        MinWidth = 200,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(10),
                        Text = answerText
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
            switch (((ComboBoxItem)cbQuestionType.SelectedItem).Content)
            {
                case "Open vraag":
                    spOpenAnswer.Visibility = Visibility.Visible;
                    divMultipleChoice.Visibility = Visibility.Hidden;
                    spMultipleChoiceControls.Visibility = Visibility.Hidden;
                    break;
                case "Multiple choice":
                    ProcessOpenAnswers();
                    spOpenAnswer.Visibility = Visibility.Hidden;
                    divMultipleChoice.Visibility = Visibility.Visible;
                    spMultipleChoiceControls.Visibility = Visibility.Visible;
                    break;
                default:
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
