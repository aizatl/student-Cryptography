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
using static System.Net.Mime.MediaTypeNames;

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
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            if (KeyTextBox.Text.Length == 8 || (IVTextBox.Text.Length == 8 && mode == "CBC")) {
                using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
                {
                    string plainText = PlainTextBox.Text.ToString();
                    string key = KeyTextBox.Text.ToString();
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    if (key == null || key.Length <= 0)
                        keyBytes = desAlg.Key;
                    string encrypted = EncryptDES(plainText, keyBytes);
                    EncryptedTextBox.Text = encrypted;
                    CiphertextBox.Text = encrypted;
                }
            }
            else
            {
                MessageBox.Show("Key and IV must be 8 bytes");
            }

        }
        private string EncryptDES(string plainText, byte[] Key) {
            byte[] encrypted;
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider()) {
                ICryptoTransform encryptor = desAlg.CreateEncryptor();
                desAlg.Key = Key;
                desAlg.Padding = PaddingMode.Zeros;
                if (mode == "CBC")
                {
                    iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);
                }
                else if(mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, null);
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
            string showResult = BitConverter.ToString(encrypted);
            return showResult;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string ciphertext = CiphertextBox.Text.ToString();
            string key = KeyTextBox.Text.ToString();
            byte[] keybyte = Encoding.UTF8.GetBytes(key);
            string plainText = DecryptDES(ciphertext, keybyte, iv);
            DecryptedTextBox.Text = plainText;
        }
        private string DecryptDES(string ciphertext, byte[] Key, byte[] IV)
        {
            string result = ciphertext.Replace("-", "");
            string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
            byte[] buffer = new byte[result.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(result.Substring(i * 2, 2), 16);
            }

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                ICryptoTransform decryptor = desAlg.CreateDecryptor();
                desAlg.Key = Key;
                desAlg.Padding = PaddingMode.Zeros;
                if (mode == "CBC")
                {
                    iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);
                }
                else if (mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, null);
                }
                
                //byte[] decryptedBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
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
            else if (mode == "CBC")
            {
                IVTextBox.Visibility = Visibility.Visible;
                textIV.Visibility = Visibility.Visible;
            }
        }
    }
}
