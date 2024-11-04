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
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            if ((KeyTextBox.Text.Length == 16 || KeyTextBox.Text.Length == 24 || KeyTextBox.Text.Length == 32) || (IVTextBox.Text.Length == 16 && mode == "CBC")) {
                using (Aes aes = Aes.Create())
                {
                    string plainText = PlainTextBox.Text.ToString();
                    string key = KeyTextBox.Text.ToString();
                    DecryptedTextBox.Text = "";
                    CiphertextBox.Text = "";
                    EncryptedTextBox.Text = "";
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    this.finalKey = keyBytes;
                    string encrypted = EncryptAES(plainText, keyBytes);
                    EncryptedTextBox.Text = encrypted;
                    CiphertextBox.Text = encrypted;
                }
            }
            else
            {
                MessageBox.Show("Key must be 16/24/32 bytes. Iv must be 16 bytes");
            }

        }

        private string EncryptAES(string plainText, byte[] Key)
        {
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor();
                aesAlg.Key = Key;
                if (mode == "CBC")
                {
                    iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.IV = iv;
                    encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                }
                else if (mode == "ECB")
                {
                    aesAlg.Mode = CipherMode.ECB;
                    encryptor = aesAlg.CreateEncryptor(aesAlg.Key, null);
                }

                
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
            return BitConverter.ToString(encrypted);
        }


        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string ciphertext = CiphertextBox.Text.ToString();
            byte[] key = Encoding.UTF8.GetBytes(KeyTextBox.Text.ToString());
            string plainText = DecryptAES(ciphertext, key, iv);
            DecryptedTextBox.Text = plainText;
        }
        private string DecryptAES(string ciphertext, byte[] Key, byte[] IV)
        {
            string result = ciphertext.Replace("-", "");
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            byte[] buffer = new byte[result.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(result.Substring(i * 2, 2), 16);
            }

            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform decryptor = aesAlg.CreateEncryptor();
                aesAlg.Key = Key;
                if (mode == "CBC")
                {
                    aesAlg.Mode = CipherMode.CBC;
                    iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    aesAlg.IV = iv;
                    decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                }
                else if (mode == "ECB")
                {
                    aesAlg.Mode = CipherMode.ECB;
                    decryptor = aesAlg.CreateDecryptor(aesAlg.Key, null);
                }


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

        private void modeChange(object sender, SelectionChangedEventArgs e)
        {
            if (IVTextBox == null) return;
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            
            if (mode == "ECB")
            {
                IVTextBox.Visibility = Visibility.Collapsed;
                textIV.Visibility = Visibility.Collapsed;
            }
            else if (mode == "CBC") {
                IVTextBox.Visibility = Visibility.Visible;
                textIV.Visibility = Visibility.Visible;
            }
        }
    }
}
