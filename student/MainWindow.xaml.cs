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
            string password = PasswordBox.Password;
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "SELECT COUNT(1) FROM Student where username = '" + username + "' and password = '" + password + "';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    int userCount = (int)command.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("Login successful!");
                        AdminMainPage mainPage1 = new AdminMainPage(username);
                        mainPage1.Show();    
                        this.Close();
                    }
                    else {
                        MessageBox.Show("Wrong login credential");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            CreateAcc createAcc = new CreateAcc();
            createAcc.Show();
            this.Close();
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
