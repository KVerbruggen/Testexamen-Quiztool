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
    /// Interaction logic for TeacherSubjectList.xaml
    /// </summary>
    public partial class TeacherSubjectList : Page
    {
        public TeacherSubjectList()
        {
            InitializeComponent();
            lblLoggedInAs.Content = "Ingelogd als: " + Main.User;
        }

        private void LoadSubjects()
        {
            foreach(Subject subject in Main.db.GetSubjects())
            {
                lbSubjects.Items.Add(subject);
            }
        }

        private void btLogout_Click(object sender, RoutedEventArgs e)
        {
            Main.Logout();
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Login());
        }

        private void btDeleteSubject_Click(object sender, RoutedEventArgs e)
        {
            Main.db.DeleteSubject((Subject)lbSubjects.SelectedItem);
            Main.db.Save();
        }

        private void btManageSubject_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectInfo((Subject)lbSubjects.SelectedItem));
        }

        private void btAddSubject_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectInfo(new Subject()));
        }
    }
}
