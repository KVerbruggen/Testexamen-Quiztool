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
    public partial class TeacherExamInfo : Page
    {
        private Exam exam;

        public TeacherExamInfo(Exam exam)
        {
            this.exam = exam;
            InitializeComponent();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
        }

        public TeacherExamInfo(Subject subject)
        {
            exam = new Exam() { Subject = subject };
            InitializeComponent();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            Main.Logout();
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Login());
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectList());
        }

    }
}
