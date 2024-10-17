using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Security.Cryptography;

namespace student
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = "aizat";// PasswordBox.Password;
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "SELECT salt, hash FROM Student WHERE username = '" + username + "'";

            //string query = "SELECT COUNT(1) FROM Student where username = '" + username + "' and password = '" + password + "';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) {
                        string salt = reader["salt"].ToString();
                        string hash = reader["hash"].ToString();

                        byte[] saltBytes = Convert.FromBase64String(salt);
                        string newHash = HashPassword(password, saltBytes);

                        if (hash == newHash) {
                            //MessageBox.Show("Login successful!");
                            AdminMainPage mainPage1 = new AdminMainPage(username);
                            mainPage1.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong login credential");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            CreateAcc createAcc = new CreateAcc();
            createAcc.Show();
            this.Close();
        }
        private string HashPassword(string password, byte[] salt)
        {
            int i = 0;
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000)) 
            {
                byte[] hash = rfc2898.GetBytes(32);
                i++;
                return Convert.ToBase64String(hash);
            }
        }
        private void LoadStudentData()
        {
            // Connection string to your SQL Server database
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";

            // SQL query to retrieve data from the Student table
            string query = "SELECT studentID as no, studentName as name FROM Student";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    connection.Open();
                    dataAdapter.Fill(dataTable);
                    connection.Close();

                    // Bind the data to the DataGrid
                    //StudentDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
