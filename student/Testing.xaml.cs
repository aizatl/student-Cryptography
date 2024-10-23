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

namespace student
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : Window
    {
        string iv = "952";
        public Testing()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            string plaintext = PlaintextTextBox.Text;
            string key1 = Key1TextBox.Text;
            string key2 = Key2TextBox.Text;
            string key3 = Key3TextBox.Text;

            if (plaintext == "" || key1 == "" || key2 == "" || key3 == "")
            {
                MessageBox.Show("Please enter plaintext and all three keys.");
                return;
            }

            
            try
            {
                string encrypted = TripleDESEncrypt(plaintext, key1, key2, key3);
                MessageBox.Show($"Encrypted Text: {encrypted}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        
        private string DESEncrypt(string data, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.UTF8.GetBytes(key.PadRight(8).Substring(0, 8)); 
                des.IV = Encoding.UTF8.GetBytes(iv.PadRight(8).Substring(0, 8));
                des.Padding = PaddingMode.PKCS7;

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                ICryptoTransform encryptor = des.CreateEncryptor();
                byte[] encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

                return Convert.ToBase64String(encryptedData);
            }
        }

        private string DESDecrypt(string data, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //key = "123";//try use key1
                des.Key = Encoding.UTF8.GetBytes(key.PadRight(8).Substring(0, 8)); 
                des.IV = Encoding.UTF8.GetBytes(iv.PadRight(8).Substring(0, 8));
                des.Padding = PaddingMode.PKCS7;

                byte[] dataBytes = Convert.FromBase64String(data);
                ICryptoTransform decryptor = des.CreateDecryptor();
                byte[] decryptedData = decryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        private string TripleDESEncrypt(string plaintext, string key1, string key2, string key3)
        {
            string encrypted = DESEncrypt(plaintext, key1);

            string decrypted = DESDecrypt(encrypted, key2);

            string tripleEncrypted = DESEncrypt(decrypted, key3);

            return tripleEncrypted;
        }
    }
}
