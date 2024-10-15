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

namespace student
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string _username;
        public Window1(string username)
        {
            InitializeComponent();
            _username = username;
            test.Text = "HI " + _username;
        }

        private void Box1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jadi");
        }

        private void Box2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Box3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Box4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
