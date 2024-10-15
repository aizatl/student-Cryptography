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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace student
{
    /// <summary>
    /// Interaction logic for AdminMainPage.xaml
    /// </summary>
    public partial class AdminMainPage : Window
    {
        private string _username;
        public AdminMainPage(string username)
        {
            InitializeComponent();
            _username = username;
            test.Text = "HI " + _username;
        }
        
        private void Box1_Click(object sender, RoutedEventArgs e)
        {
            //view all complain
            Complain complain = new Complain(_username);
            complain.Show();
            this.Close();
        }

        private void Box2_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
