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
    /// Interaction logic for DES.xaml
    /// </summary>
    public partial class DES : Window
    {
        byte[] finalKey = new byte[8];//store key because for aes, both sender and receiver use same key
        byte[] iv = new byte[8];
        public DES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                string plainText = PlainTextBox.Text.ToString();
                string key = KeyTextBox.Text.ToString();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                if (key == null || key.Length <= 0)
                    keyBytes = desAlg.Key;
                byte[] finalKeyBytes = new byte[8];
                Buffer.BlockCopy(keyBytes, 0, finalKeyBytes, 0, Math.Min(keyBytes.Length, finalKeyBytes.Length));
                finalKey = finalKeyBytes;
                desAlg.GenerateIV(); // Generate a random IV
                iv = desAlg.IV;
                string encrypted = EncryptDES(plainText, finalKeyBytes, iv);
                EncryptedTextBox.Text = encrypted;
                CiphertextBox.Text = encrypted;
            }
        }
        private string EncryptDES(string plainText, byte[] Key, byte[] IV) {
            byte[] encrypted;
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider()) {
                desAlg.Key = Key;
                desAlg.IV = IV;
                ICryptoTransform encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);
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

            string plainText = DecryptDES(ciphertext, finalKey, iv);
            DecryptedTextBox.Text = plainText;
        }
        private string DecryptDES(string ciphertext, byte[] Key, byte[] IV)
        {
            byte[] buffer = Convert.FromBase64String(ciphertext);

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = Key;
                desAlg.IV = IV;

                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

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
