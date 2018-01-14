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
    public partial class TeacherTopicInfo : Page
    {
        private Topic topic;

        public TeacherTopicInfo(Topic topic)
        {
            this.topic = topic;
            InitializeComponent();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
        }

        public TeacherTopicInfo(Subject subject)
        {
            topic = new Topic() { Subject = subject };
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
