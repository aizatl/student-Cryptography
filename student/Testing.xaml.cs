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
        byte[] iv = new byte[8];
        public Testing()
        {
            InitializeComponent();
        }

        
        private byte[] EncryptDES(byte[] dataByte, byte[] key, byte[] iv)
        {
            //using (MemoryStream mStream = new MemoryStream())
            //{
            //    // Create a new DES object.
            //    using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            //    // Create a DES encryptor from the key and IV
            //    using (ICryptoTransform encryptor = desAlg.CreateEncryptor(key, iv))
            //    // Create a CryptoStream using the MemoryStream and encryptor
            //    using (var cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
            //    {


            //        // Write the byte array to the crypto stream and flush it.
            //        cStream.Write(dataByte, 0, dataByte.Length);

            //        // Ending the using statement for the CryptoStream completes the encryption.
            //    }

            //    // Get an array of bytes from the MemoryStream that holds the encrypted data.
            //    byte[] ret = mStream.ToArray();

            //    // Return the encrypted buffer.
            //    return ret;
            //}
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.IV = iv;
                desAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataByte, 0, dataByte.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedData = msEncrypt.ToArray();
                    return encryptedData;
                }
            }
        }

        private byte[] DecryptDES(byte[] buffer, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.IV = iv;
                desAlg.Padding = PaddingMode.PKCS7;
                //desAlg.Padding = PaddingMode.None;

                // Use TransformFinalBlock to perform raw decryption on the byte array
                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                return decryptedBytes;
            }
            //using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            //{
            //    desAlg.Key = key;
            //    desAlg.IV = iv;
            //    //desAlg.Padding = PaddingMode.PKCS7;
            //    desAlg.Padding = PaddingMode.None;
            //    ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

            //    using (MemoryStream msDecrypt = new MemoryStream(dataByte))

            //    {
            //        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            //        {
            //            using (MemoryStream msDecrypted = new MemoryStream())
            //            {
            //                csDecrypt.CopyTo(msDecrypted);
            //                //return msDecrypted.ToArray();
            //                byte[] encryptedData = msDecrypted.ToArray();
            //                return encryptedData;

            //            }
            //        }
            //    }
            //}

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
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                byte[] predefinedIV = new byte[8] { 3, 6, 3, 1, 5, 9, 7, 8 };
                desAlg.IV = predefinedIV;
                this.iv = desAlg.IV;
            }
            if (KeyTextBox1.Text.Length == 8 )
            {
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                try
                {
                    byte[] result1 = EncryptDES(plaintextByte, keyBytes1, iv);
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
                    byte[] result2 = EncryptDES(result1Byte, keyBytes2, iv);
                    //byte[] result2 = DecryptDES(result1Byte, keyBytes2, iv);
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
                    byte[] result3 = EncryptDES(result2Byte, keyBytes3, iv);
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
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(KeyTextBox3.Text);
                try { 
                    byte[] result4 = DecryptDES(result3Byte, keyBytes3, iv);//this decrypt
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
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(KeyTextBox2.Text);
                try
                {
                    byte[] result5 = DecryptDES(result4Byte, keyBytes2, iv);
                    //byte[] result5 = EncryptDES(result4Byte, keyBytes2, iv);
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
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                try
                {
                    byte[] result6 = DecryptDES(result5Byte, keyBytes1, iv);
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
    }
}
