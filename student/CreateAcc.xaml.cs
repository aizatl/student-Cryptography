using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data.SqlClient;
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
    /// Interaction logic for CreateAcc.xaml
    /// </summary>
    public partial class CreateAcc : Window
    {
        public CreateAcc()
        {
            InitializeComponent();
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            string name = NameTextBox.Text;
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            byte[] saltBytes = GenerateSalt();
            string salt = Convert.ToBase64String(saltBytes);
            string hash = HashPassword(password, saltBytes);
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "INSERT INTO STUDENT(studentName, username, password)" +
                " VALUES('" + name +"','" + username + "','" + password + "')";
            string checkUsername = "SELECT COUNT(1) FROM Student where username = '" + username + "';";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(checkUsername, connection);
                try
                {
                    connection.Open();
                    int userCount = (int)command.ExecuteScalar();
                    if (userCount == 0)
                    {
                        using (SqlCommand insertCommand = new SqlCommand(query, connection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }
                        MessageBox.Show("Succesfully successfully.");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else {
                        MessageBox.Show("Username Exist.");
                        UsernameBox.Focus();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // 16 bytes = 128 bits
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Method to hash the password with the salt
        private string HashPassword(string password, byte[] salt)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
            {
                byte[] hash = rfc2898.GetBytes(32); // 32 bytes = 256 bits hash
                return Convert.ToBase64String(hash);
            }
        }
    }
}
