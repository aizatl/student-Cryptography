using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace student
{
    public partial class testingFromQariLaptop : Window
    {
        byte[] aizat = new byte[8];
        public testingFromQariLaptop()
        {
            InitializeComponent();
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ciphertext = CiphertextTextBox.Text;
                string key = KeyTextBox.Text;

                if (key.Length != 8)
                {
                    MessageBox.Show("Key must be exactly 8 characters long.");
                    return;
                }

                string plaintext = DecryptDES(ciphertext, key);
                PlaintextTextBox.Text = plaintext;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during decryption: " + ex.Message);
            }
        }

        private string DecryptDES(string cipherText, string key)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = keyBytes;
                    des.IV = keyBytes; // Simplified IV for testing
                    des.Padding = PaddingMode.None; // No padding to avoid errors

                    ICryptoTransform decryptor = des.CreateDecryptor();

                    // Attempt to decrypt
                    byte[] resultArray = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                    return Encoding.UTF8.GetString(resultArray);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception if needed
                return "Decryption failed or produced invalid data: " + ex.Message;
            }
        }




    }
}
