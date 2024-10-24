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

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyTextBox1.Text != null)
            {
                string plainText = PlainTextBox.Text;
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text);
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text);
                byte[] finalKeyBytes1 = new byte[8];
                byte[] finalKeyBytes2 = new byte[8];
                byte[] finalKeyBytes3 = new byte[8];
                Buffer.BlockCopy(keyBytes1, 0, finalKeyBytes1, 0, Math.Min(keyBytes1.Length, finalKeyBytes1.Length));
                Buffer.BlockCopy(keyBytes2, 0, finalKeyBytes2, 0, Math.Min(keyBytes2.Length, finalKeyBytes2.Length));
                Buffer.BlockCopy(keyBytes3, 0, finalKeyBytes3, 0, Math.Min(keyBytes3.Length, finalKeyBytes3.Length));
                try
                {
                    string ciphertext = DoWork(plainText, finalKeyBytes1, finalKeyBytes2, finalKeyBytes3);
                    //string ciphertext = DoWork(plainText, keyBytes1, keyBytes2, keyBytes3);
                    EncryptedTextBox.Text = ciphertext;
                    CiphertextBox.Text = ciphertext;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }
        private string DoWork(string plainText, byte[] keyBytes1, byte[] keyBytes2, byte[] keyBytes3)
        {
            byte[] plaintextByte = Encoding.UTF8.GetBytes(plainText);

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.GenerateIV();
                //byte[] predefinedIV = new byte[8] { 3, 6, 3, 1, 5, 9, 7, 8 }; 
                //desAlg.IV = predefinedIV;
                iv = desAlg.IV;
                desAlg.Padding = PaddingMode.PKCS7;
            }
            byte[] result1 = EncryptDES(plaintextByte, keyBytes1, iv);
            //byte[] result2 = DecryptDES(result1, keyBytes1, iv);
            byte[] result2 = EncryptDES(result1, keyBytes2, iv);
            byte[] result3 = EncryptDES(result2, keyBytes3, iv);

            string a = Convert.ToBase64String(result3);
            string b = Convert.ToBase64String(iv);
            return Convert.ToBase64String(result3);
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

        private byte[] DecryptDES(byte[] dataByte, byte[] key, byte[] iv)
        {
            //byte[] decrypted = new byte[dataByte.Length];
            //int offset = 0;

            //// Create a new MemoryStream using the provided array of encrypted data.
            //using (MemoryStream mStream = new MemoryStream(dataByte))
            //{
            //    // Create a new DES object.
            //    using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            //    // Create a DES decryptor from the key and IV
            //    using (ICryptoTransform decryptor = desAlg.CreateDecryptor(key, iv))
            //    // Create a CryptoStream using the MemoryStream and decryptor
            //    using (var cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
            //    {
            //        // Keep reading from the CryptoStream until it finishes (returns 0).
            //        int read = 1;

            //        while (read > 0)
            //        {
            //            read = cStream.Read(decrypted, offset, decrypted.Length - offset);
            //            offset += read;
            //        }
            //    }
            //}

            //// Convert the buffer into a string and return it.
            //return Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(decrypted, 0, offset));
            //here aizat
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = key;
                desAlg.IV = iv;
                desAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(dataByte))

                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msDecrypted = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msDecrypted);
                            //return msDecrypted.ToArray();
                            byte[] encryptedData = msDecrypted.ToArray();
                            return encryptedData;

                        }
                    }
                }
            }

        }
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CiphertextBox.Text) && !string.IsNullOrEmpty(EncryptedTextBox.Text))
            {
                string ciphertext = CiphertextBox.Text;
                byte[] keyBytes1 = Encoding.UTF8.GetBytes(KeyTextBox1.Text);
                byte[] keyBytes2 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text);
                byte[] keyBytes3 = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text);
                byte[] finalKeyBytes1 = new byte[8];
                byte[] finalKeyBytes2 = new byte[8];
                byte[] finalKeyBytes3 = new byte[8];
                Buffer.BlockCopy(keyBytes1, 0, finalKeyBytes1, 0, Math.Min(keyBytes1.Length, finalKeyBytes1.Length));
                Buffer.BlockCopy(keyBytes2, 0, finalKeyBytes2, 0, Math.Min(keyBytes2.Length, finalKeyBytes2.Length));
                Buffer.BlockCopy(keyBytes3, 0, finalKeyBytes3, 0, Math.Min(keyBytes3.Length, finalKeyBytes3.Length));
                
                try
                {
                    string plaintext = DoWorkDec(ciphertext, finalKeyBytes1, finalKeyBytes2, finalKeyBytes3);
                    //string ciphertext = DoWork(plainText, keyBytes1, keyBytes2, keyBytes3);
                    
                    DecryptedTextBox.Text = plaintext;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private string DoWorkDec(string ciphertext, byte[] keyBytes1, byte[] keyBytes2, byte[] keyBytes3)
        {
            byte[] ciphertextByte = Convert.FromBase64String(ciphertext);
            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                //desAlg.GenerateIV();
                //iv = desAlg.IV;
                desAlg.Padding = PaddingMode.PKCS7;
            }
            byte[] result1 = DecryptDES(ciphertextByte, keyBytes3, iv);
            //byte[] result2 = EncryptDES(result1, keyBytes2, iv);
            byte[] result2 = DecryptDES(result1, keyBytes2, iv);

            byte[] result3 = DecryptDES(result2, keyBytes1, iv);


            return Encoding.UTF8.GetString(result3);
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }
    }
}
