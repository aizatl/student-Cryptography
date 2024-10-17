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
        public TDES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyTextBox1.Text != null) {
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text);
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text);
                string plainText = PlainTextBox.Text;
                byte[] finalKeyByte1 = new byte[8];
                byte[] finalKeyByte2 = new byte[8];
                byte[] finalKeyByte3 = new byte[8];
                Buffer.BlockCopy(keyBytes1, 0, finalKeyByte1, 0, Math.Min(keyBytes1.Length, finalKeyByte1.Length));
                Buffer.BlockCopy(keyBytes2, 0, finalKeyByte2, 0, Math.Min(keyBytes2.Length, finalKeyByte2.Length));
                Buffer.BlockCopy(keyBytes3, 0, finalKeyByte3, 0, Math.Min(keyBytes3.Length, finalKeyByte3.Length));
                string ciphertext = EncryptTDES(plainText, finalKeyByte1, finalKeyByte2, finalKeyByte3);
                EncryptedTextBox.Text = ciphertext;
                CiphertextBox.Text = ciphertext;

            }

        }
        private string EncryptTDES(string plainText, byte[] Key1, byte[] Key2, byte[] Key3)
        {
            byte[] encrypted;
            byte[] combinedKey = new byte[24];
            Buffer.BlockCopy(Key1, 0, combinedKey, 0, 8);
            Buffer.BlockCopy(Key2, 0, combinedKey, 8, 8);
            Buffer.BlockCopy(Key3, 0, combinedKey, 16, 8);
            using (TripleDESCryptoServiceProvider tdesAlg = new TripleDESCryptoServiceProvider())
            {
                tdesAlg.Key = combinedKey;

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

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }
    }
}
