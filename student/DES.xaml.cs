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
        byte[] iv = new byte[8];
        public DES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyTextBox.Text.Length == 8) {
                using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
                {
                    string plainText = PlainTextBox.Text.ToString();
                    string key = KeyTextBox.Text.ToString();
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    if (key == null || key.Length <= 0)
                        keyBytes = desAlg.Key;
                    
                    //finalKey = keyBytes;
                    //desAlg.GenerateIV(); // Generate a random IV
                    byte[] predefinedIV = new byte[8] { 3, 6, 3, 1, 5, 9, 7, 8 };
                    desAlg.IV = predefinedIV;
                    iv = desAlg.IV;
                    string encrypted = EncryptDES(plainText, keyBytes, iv);
                    EncryptedTextBox.Text = encrypted;
                    CiphertextBox.Text = encrypted;
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
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
            string showResult = BitConverter.ToString(encrypted);
            return showResult;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string ciphertext = CiphertextBox.Text.ToString();
            string key = KeyTextBox.Text.ToString();//.Replace("1","9");
            byte[] keybyte = Encoding.UTF8.GetBytes(key);
            string plainText = DecryptDES(ciphertext, keybyte, iv);
            DecryptedTextBox.Text = plainText;
        }
        private string DecryptDES1(string ciphertext, byte[] Key, byte[] IV)
        {
            string result = ciphertext.Replace("-", "");

            byte[] buffer = new byte[result.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(result.Substring(i * 2, 2), 16);
            }

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = Key;
                //desAlg.GenerateKey();
                //Key = desAlg.Key;
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
        private string DecryptDES(string ciphertext, byte[] Key, byte[] IV)
        {
            // Remove hyphens from the ciphertext
            string result = ciphertext.Replace("-", "");

            // Convert hex string to byte array
            byte[] buffer = new byte[result.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(result.Substring(i * 2, 2), 16);
            }

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = Key;
                desAlg.IV = IV;
                //desAlg.Padding = PaddingMode.None;

                // Use TransformFinalBlock to perform raw decryption on the byte array
                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                // Convert decrypted bytes directly to string, ignoring any potential errors
                string aizat = Encoding.UTF8.GetString(decryptedBytes);
                string aiman =  BitConverter.ToString(decryptedBytes);
                return Encoding.UTF8.GetString(decryptedBytes);
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
