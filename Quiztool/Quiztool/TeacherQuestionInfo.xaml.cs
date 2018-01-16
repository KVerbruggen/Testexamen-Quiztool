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
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
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
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherTopicInfo(question.Topic));
        }
    }
}
