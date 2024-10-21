using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for TDES.xaml
    /// </summary>
    public partial class TDES : Window
    {
        byte[] iv = new byte[8];
        byte[] combinedKey = new byte[24];//3 keys means 3*8
        public TDES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyTextBox1.Text != null) {
                string plainText = PlainTextBox.Text;
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text);
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text);

                Buffer.BlockCopy(Encoding.UTF8.GetBytes(KeyTextBox1.Text.PadRight(8).Substring(0, 8)), 0, combinedKey, 0, 8);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes((string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text).PadRight(8).Substring(0, 8)), 0, combinedKey, 8, 8);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes((string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text).PadRight(8).Substring(0, 8)), 0, combinedKey, 16, 8);

                string ciphertext = EncryptTDES(plainText, combinedKey);
                EncryptedTextBox.Text = ciphertext;
                CiphertextBox.Text = ciphertext;

            }

        }
        private string EncryptTDES(string plainText, byte[] combinedKey)
        {
            byte[] encrypted;
            using (TripleDESCryptoServiceProvider tdesAlg = new TripleDESCryptoServiceProvider())
            {
                tdesAlg.Key = combinedKey;
                tdesAlg.GenerateIV(); 
                this.iv = tdesAlg.IV;

                ICryptoTransform encryptor = tdesAlg.CreateEncryptor(tdesAlg.Key, tdesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CiphertextBox.Text) && !string.IsNullOrEmpty(EncryptedTextBox.Text)) {
                string ciphertext = CiphertextBox.Text;
                string plaintext = DecryptTDES(ciphertext, combinedKey);
                DecryptedTextBox.Text = plaintext;
            }
        }
        private string DecryptTDES(string cipherText, byte[] combinedKey)
        {
            byte[] decrypted;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (TripleDESCryptoServiceProvider tdesAlg = new TripleDESCryptoServiceProvider())
            {
                tdesAlg.Key = combinedKey;
                tdesAlg.IV = iv;

                ICryptoTransform decryptor = tdesAlg.CreateDecryptor(tdesAlg.Key, tdesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                        }
                    }
                }
            }
            return Encoding.UTF8.GetString(decrypted);
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }
    }
}
