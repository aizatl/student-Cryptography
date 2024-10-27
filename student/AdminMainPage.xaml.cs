using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            //test.Text = "HI " + _username;
            test.Visibility = Visibility.Collapsed;
        }
        private void AES_Click(object sender, RoutedEventArgs e)
        {
            AES aes = new AES();
            aes.Show();
            this.Close();
        }

        private void DES_Click(object sender, RoutedEventArgs e)
        {
            DES des = new DES();
            des.Show();
            this.Close();
        }

        private void TDES_Click(object sender, RoutedEventArgs e)
        {
            TDES tdes = new TDES();
            tdes.Show();
            this.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new MainWindow();
            MW.Show();
            this.Close();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            Testing t = new Testing();
            //testingFromQariLaptop t = new testingFromQariLaptop();
            t.Show();
            this.Close();
        }
    }
}
