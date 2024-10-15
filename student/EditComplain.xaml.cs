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

namespace student
{
    /// <summary>
    /// Interaction logic for EditComplain.xaml
    /// </summary>
    public partial class EditComplain : Window
    {
        private int _id;
        private string _username;
        public EditComplain(int id, string username)
        {
            _id = id;
            _username = username;
            InitializeComponent();
            AssignValue(id);
        }
        private void AssignValue(int id)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "SELECT c.complainID, c.complainTitle, c.complainDetail, c.complainDate, c.status, c.dateClose, s.name" +
                "   FROM complain c " +
                "   left join student s on s.studentID = c.studentID" +
                "   where complainID = " + id;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtComplainID.Text = reader["complainID"].ToString();
                                txtComplainTitle.Text = reader["complainTitle"].ToString();
                                txtComplainDetail.Text = reader["complainDetail"].ToString();
                                dpComplainDate.SelectedDate = reader["complainDate"] as DateTime?;
                                string status = reader["status"].ToString();
                                txtStatus.Text = status;
                                dpDateClose.SelectedDate = reader["dateClose"] as DateTime?;
                                studentName.Text = reader["name"].ToString();

                                if (status.Trim().Equals("Closed", StringComparison.OrdinalIgnoreCase))
                                {
                                    txtComplainTitle.IsReadOnly = true;
                                    txtComplainDetail.IsReadOnly = true;
                                    dpComplainDate.IsEnabled = false;
                                    txtStatus.IsReadOnly = true;
                                    dpDateClose.IsEnabled = false;
                                    studentName.IsReadOnly = true;
                                    dpDateClose.Visibility = Visibility.Visible;
                                    textDateClose.Visibility = Visibility.Visible;
                                    closeBtn.Visibility = Visibility.Collapsed;
                                    saveBtn.Visibility = Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Record not found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string complainTitle = txtComplainTitle.Text;
            string complainDetail = txtComplainDetail.Text;
            string complainDate = dpComplainDate.SelectedDate.ToString();
            string complainId = txtComplainID.Text;

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "UPDATE complain" +
                " SET complainTitle = '" + complainTitle + "'" +
                " ,complainDetail = '" + complainDetail + "'" +
                " ,complainDate = '" + complainDate + "'" +
                "WHERE complainID = " + complainId;

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
                            MessageBox.Show("Complain saved successfully.");
                            Complain complain = new Complain(_username);
                            complain.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("No complain saved");
                            Complain complain = new Complain(_username);
                            complain.Show();
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            string todayDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";
            string query = "UPDATE complain" +
                "   SET status = 'Closed', dateClose = '" + todayDate + "'" +
                "   where complainID = " + _id;

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
                            MessageBox.Show("Complain closed successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No record found");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Complain complain = new Complain(_username);
            complain.Show();
            this.Close();
        }
    }
}
