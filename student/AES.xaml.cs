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
    /// Interaction logic for AES.xaml
    /// </summary>
    public partial class AES : Window
    {
        byte[] finalKey = new byte[32];//store key because for aes, both sender and receiver use same key
        byte[] iv = new byte[32];
        public AES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            using (Aes aes = Aes.Create()) {
                string plainText = PlainTextBox.Text.ToString();
                string key = KeyTextBox.Text.ToString();
                DecryptedTextBox.Text = "";
                CiphertextBox.Text = "";
                EncryptedTextBox.Text = "";
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                if (key == null || key.Length <= 0)
                    keyBytes = aes.Key;
                byte[] finalKeyBytes = new byte[32];
                Buffer.BlockCopy(keyBytes, 0, finalKeyBytes, 0, Math.Min(keyBytes.Length, finalKeyBytes.Length));
                this.finalKey = finalKeyBytes;
                aes.GenerateIV(); // Generate a random IV
                iv = aes.IV;
                string encrypted = EncryptAES(plainText, finalKeyBytes, iv);
                EncryptedTextBox.Text = encrypted;
                CiphertextBox.Text = encrypted;
            }
        }

        private string EncryptAES(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }


        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string ciphertext = CiphertextBox.Text.ToString();
            string key = KeyTextBox.Text.ToString();

            string plainText = DecryptAES(ciphertext, finalKey, iv);
            DecryptedTextBox.Text = plainText;
        }
        private string DecryptAES(string ciphertext, byte[] Key, byte[] IV)
        {
            byte[] buffer = Convert.FromBase64String(ciphertext);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }
    }
}
