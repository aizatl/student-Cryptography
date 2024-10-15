using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace student
{
    /// <summary>
    /// Interaction logic for AddComplainPage.xaml
    /// </summary>
    public partial class AddComplainPage : Window
    {
        private string _username;
        public AddComplainPage(string username)
        {
            _username = username;
            InitializeComponent();
            LoadStudentData();
            
        }
        private void LoadStudentData()
        {
            
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "SELECT studentID, name FROM student";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            int id = (int)reader["studentID"];
                            string name = reader["name"].ToString();
                            cboStudentName.Items.Add(id + "-" + name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading students: " + ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Complain C = new Complain(_username);
            C.Show();
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string complainTitle = txtComplainTitle.Text.ToString();
            string complainDetail = txtComplainDetail.Text.ToString();
            DateTime? complainDate = dpComplainDate.SelectedDate;
            string complainBy = cboStudentName.SelectedValue.ToString();
            complainBy = complainBy.Split('-')[0].Trim();
            
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "INSERT INTO complain (complainTitle, complainDetail, complainDate, studentID,status)" +
                "VALUES (@complainTitle, @complainDetail, @complainDate, @studentID, @status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@complainTitle", complainTitle);
                        command.Parameters.AddWithValue("@complainDetail", complainDetail);
                        command.Parameters.AddWithValue("@complainDate", complainDate);
                        command.Parameters.AddWithValue("@studentID", complainBy);
                        command.Parameters.AddWithValue("@status", "Open");

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0) {
                            MessageBox.Show("Complaint added successfully.");
                            Complain C = new Complain(_username);
                            C.Show();
                            this.Close();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading students: " + ex.Message);
                }
            }

        }
    }
}
