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
using System.Text.RegularExpressions;

namespace Quiztool
{
    /// <summary>
    /// Interaction logic for TeacherSubjectInfo.xaml
    /// </summary>
    public partial class TeacherExamInfo : Page
    {
        private Exam exam;

        public TeacherExamInfo(Exam exam)
        {
            this.exam = exam;
            InitializeComponent();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
            lblQuestionsInTopics.Content = "Totaal aantal vragen in onderwerpen: " + exam.GetTotalQuestions();
        }

        private void ForceNumberInput(object sender, TextCompositionEventArgs e)
        {
            // Source: https://stackoverflow.com/questions/42480514/c-sharp-wpf-how-to-use-only-numeric-value-in-a-textbox
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            Main.db.RejectChanges();
            Main.Logout();
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Login());
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            Main.db.RejectChanges(exam);
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectInfo(exam.Subject));
        }

        private void btDone_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectInfo(exam.Subject));
        }
    }
}
