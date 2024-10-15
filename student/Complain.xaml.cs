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
using System.Windows.Shapes;

namespace student
{
    /// <summary>
    /// Interaction logic for Complain.xaml
    /// </summary>
    public partial class Complain : Window
    {
        private string _username;
        public Complain(string username)
        {
            InitializeComponent();
            _username = username;
            LoadData();
        }
        private void LoadData()
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "SELECT c.complainID, c.complainTitle, c.complainDetail, c.status, s.name " +
                "FROM complain c LEFT JOIN student s ON s.studentID = c.studentID ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                complainDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataRowView rowView = button.DataContext as DataRowView;
            int complainID = (int)rowView["complainID"];
            EditComplain ed = new EditComplain(complainID, _username);
            ed.Show();
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataRowView rowView = button.DataContext as DataRowView;
            int complainID = (int)rowView["complainID"];
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result == MessageBoxResult.Yes) {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
                string query = "DELETE FROM complain WHERE complainID = " + complainID;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully.");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Fail to delete.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            AddComplainPage ACP = new AddComplainPage(_username);
            ACP.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage(_username);
            AMP.Show();
            this.Close();
        }
    }
}
