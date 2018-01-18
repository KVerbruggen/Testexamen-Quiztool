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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Application.Current.MainWindow).Navigate(new Index());
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Main.Login(tbName.Text, tbPassword.Password))
            {
                ((NavigationWindow)Application.Current.MainWindow).Navigate(new TeacherSubjectList());
            }
            else
            {
                lblBadLogin.Visibility = Visibility.Visible;
                tbPassword.Clear();
            }
        }
    }
}
