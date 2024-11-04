using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using System.Collections;
using System.Net;

namespace student
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : Window
    {
        //byte[] iv = new byte[8] { 3, 6, 3, 1, 5, 9, 7, 8 };
        public Testing()
        {
            InitializeComponent();
        }

        
        private byte[] EncryptDESPAD(byte[] dataByte, byte[] key, string mode, byte[] iv)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.Padding = PaddingMode.PKCS7;
                
                ICryptoTransform encryptor = desAlg.CreateEncryptor();
                if (mode == "CBC")
                {
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);
                }
                else if (mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, null);
                }
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataByte, 0, dataByte.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedData = msEncrypt.ToArray();
                    return encryptedData;//this should 8
                }
            }
        }
        private byte[] EncryptDESNoPad(byte[] dataByte, byte[] key, string mode, byte[] iv)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.Padding = PaddingMode.None;
                
                ICryptoTransform encryptor = desAlg.CreateEncryptor();
                if (mode == "CBC")
                {
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);
                }
                else if (mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    encryptor = desAlg.CreateEncryptor(desAlg.Key, null);
                }
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataByte, 0, dataByte.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedData = msEncrypt.ToArray();
                    return encryptedData;//this should 8
                }
            }
        }

        private byte[] DecryptDESPAD(byte[] buffer, byte[] key, string mode, byte[] iv)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = desAlg.CreateDecryptor();
                if (mode == "CBC")
                {
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);
                }
                else if (mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, null);
                }

                byte[] decryptedBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                return decryptedBytes;//this should 8
            }
        }
        private byte[] DecryptDESNoPAD(byte[] buffer, byte[] key, string mode, byte[] iv)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.Padding = PaddingMode.None;
                ICryptoTransform decryptor = desAlg.CreateDecryptor();
                if (mode == "CBC")
                {
                    desAlg.IV = iv;
                    desAlg.Mode = CipherMode.CBC;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);
                }
                else if (mode == "ECB")
                {
                    desAlg.Mode = CipherMode.ECB;
                    decryptor = desAlg.CreateDecryptor(desAlg.Key, null);
                }
                byte[] decryptedBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                return decryptedBytes;//this should 8
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }

        private void EncryptKey1Button_Click(object sender, RoutedEventArgs e)
        {
            string plainText = PlainTextBox.Text;
            byte[] plaintextByte = Encoding.UTF8.GetBytes(plainText);
            if (KeyTextBox1.Text.Length == 8 )
            {
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                try
                {
                    string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    byte[] result1 = EncryptDESPAD(plaintextByte, keyBytes1, mode, iv);
                    string showResult1 = BitConverter.ToString(result1);
                    Key1ResultTextBox.Text = showResult1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
        }

        private void EncryptKey2Button_Click(object sender, RoutedEventArgs e)
        {
            string result1 = Key1ResultTextBox.Text.Replace("-", "");
            
            byte[] result1Byte = new byte[result1.Length / 2];
            for (int i = 0; i < result1Byte.Length; i++)
            {
                result1Byte[i] = Convert.ToByte(result1.Substring(i * 2, 2), 16);
            }
            
            if (KeyTextBox1.Text.Length == 8)
            {
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(KeyTextBox2.Text);
                try
                {
                    string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    byte[] result2 = DecryptDESNoPAD(result1Byte, keyBytes2, mode, iv);
                    string showResult2 = BitConverter.ToString(result2);
                    Key2ResultTextBox.Text = showResult2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
        }
        private void EncryptKey3Button_Click(object sender, RoutedEventArgs e)
        {
            string result2 = Key2ResultTextBox.Text.Replace("-", ""); ;

            byte[] result2Byte = new byte[result2.Length / 2];
            for (int i = 0; i < result2Byte.Length; i++)
            {
                result2Byte[i] = Convert.ToByte(result2.Substring(i * 2, 2), 16);
            }
            if (KeyTextBox1.Text.Length == 8)
            {
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(KeyTextBox3.Text);
                try
                {
                    byte[] result3 = new byte[8];
                    string mode = ((ComboBoxItem)ModeComboBox.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBox.Text);
                    result3 = EncryptDESNoPad(result2Byte, keyBytes3, mode, iv);
                    string showResult2 = BitConverter.ToString(result3);
                    Key3ResultTextBox.Text = showResult2;
                    CiphertextBox.Text = showResult2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
        }

        private void DecryptKey3Button_Click(object sender, RoutedEventArgs e)
        {
            string result3 = CiphertextBox.Text.Replace("-", "");

            byte[] result3Byte = new byte[result3.Length / 2];
            for (int i = 0; i < result3Byte.Length; i++)
            {
                result3Byte[i] = Convert.ToByte(result3.Substring(i * 2, 2), 16);
            }

            if (KeyTextBox1.Text.Length == 8)
            {
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(KeyTextBox3Dec.Text);
                try {
                    string mode = ((ComboBoxItem)ModeComboBoxDec.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBoxDec.Text);
                    byte[] result4 = DecryptDESNoPAD(result3Byte, keyBytes3, mode, iv);//this decrypt
                    string showResult4 = BitConverter.ToString(result4);
                    DecryptKey3ResultTextBox.Text = showResult4;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
        }

        private void DecryptKey2Button_Click(object sender, RoutedEventArgs e)
        {
            string result4 = DecryptKey3ResultTextBox.Text.Replace("-", ""); ;

            byte[] result4Byte = new byte[result4.Length / 2];
            for (int i = 0; i < result4Byte.Length; i++)
            {
                result4Byte[i] = Convert.ToByte(result4.Substring(i * 2, 2), 16);
            }
            if (KeyTextBox1.Text.Length == 8)
            {
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(KeyTextBox2Dec.Text);
                try
                {
                    string mode = ((ComboBoxItem)ModeComboBoxDec.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBoxDec.Text);
                    byte[] result5 = EncryptDESNoPad(result4Byte, keyBytes2, mode, iv);
                    string showResult5 = BitConverter.ToString(result5);
                    DecryptKey2ResultTextBox.Text = showResult5;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
        }

        private void DecryptKey1Button_Click(object sender, RoutedEventArgs e)
        {
            string result5 = DecryptKey2ResultTextBox.Text.Replace("-", "");

            byte[] result5Byte = new byte[result5.Length / 2];
            for (int i = 0; i < result5Byte.Length; i++)
            {
                result5Byte[i] = Convert.ToByte(result5.Substring(i * 2, 2), 16);
            }

            if (KeyTextBox1.Text.Length == 8)
            {
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1Dec.Text);
                try
                {
                    string mode = ((ComboBoxItem)ModeComboBoxDec.SelectedItem).Content.ToString();
                    byte[] iv = Encoding.UTF8.GetBytes(IVTextBoxDec.Text);
                    byte[] result6 = DecryptDESPAD(result5Byte, keyBytes1, mode, iv);
                    //string showResult6 = BitConverter.ToString(result6);
                    string decryptedText = Encoding.UTF8.GetString(result6);
                    DecryptKey1ResultTextBox.Text = decryptedText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Key must be 8 bytes");
            }
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

        private void modeChangeDec(object sender, SelectionChangedEventArgs e)
        {
            if (IVTextBoxDec == null) return;
            string mode = ((ComboBoxItem)ModeComboBoxDec.SelectedItem).Content.ToString();

            if (mode == "ECB")
            {
                IVTextBoxDec.Visibility = Visibility.Collapsed;
                textIVDec.Visibility = Visibility.Collapsed;
            }
            else if (mode == "CBC")
            {
                IVTextBoxDec.Visibility = Visibility.Visible;
                textIVDec.Visibility = Visibility.Visible;
            }
        }
    }
}
