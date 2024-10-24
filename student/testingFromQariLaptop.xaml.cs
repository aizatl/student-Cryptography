using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace student
{
    public partial class testingFromQariLaptop : Window
    {
        byte[] aizat = new byte[8];
        public testingFromQariLaptop()
        {
            InitializeComponent();
        }

        // Method to convert key string into a byte array
        private byte[] GetKeyBytes(string key)
        {
            // Ensure the key is at least 8 bytes long for DES (can truncate or pad if needed)
            return Encoding.UTF8.GetBytes(key.PadRight(8, '0').Substring(0, 8));
        }

        // Method to generate a random IV (Initialization Vector)
        private byte[] GenerateIV()
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.GenerateIV();
                return des.IV;
            }
        }

        private void Perform3DES_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user inputs
                string plainText = PlainTextBox.Text;
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] key1 = GetKeyBytes(Key1TextBox.Text);
                byte[] key2 = GetKeyBytes(Key2TextBox.Text);
                byte[] key3 = GetKeyBytes(Key3TextBox.Text);

                // Generate a random IV for the encryption/decryption process
                byte[] iv = GenerateIV();
                this.aizat = iv;
                // Encrypt with Key 1
                byte[] step1 = EncryptDES(plainTextBytes, key1, iv);

                // Decrypt with Key 2
                byte[] step2 = DecryptDES(step1, key2, iv);

                // Encrypt with Key 3
                byte[] finalResult = EncryptDES(step2, key3, iv);

                // Convert final result to Base64 and display it
                ResultTextBox.Text = Convert.ToBase64String(finalResult);
                CipherTextBox.Text = Convert.ToBase64String(finalResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Overloaded method for DES encryption where input is a byte array
        private byte[] EncryptDES(byte[] data, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                ICryptoTransform encryptor = des.CreateEncryptor(des.Key, des.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        // Method for DES decryption
        private byte[] DecryptDES(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                ICryptoTransform decryptor = des.CreateDecryptor(des.Key, des.IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream output = new MemoryStream())
                        {
                            // Read from CryptoStream into the output MemoryStream
                            cs.CopyTo(output);
                            cs.FlushFinalBlock(); // Ensure all data is flushed
                            return output.ToArray();
                        }
                    }
                }
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the Base64-encoded ciphertext
                string cipherTextBase64 = CipherTextBox.Text;
                byte[] cipherText = Convert.FromBase64String(cipherTextBase64);

                // Retrieve keys (they must match the keys used during encryption)
                byte[] key1 = GetKeyBytes(Key1TextBox.Text);
                byte[] key2 = GetKeyBytes(Key2TextBox.Text);
                byte[] key3 = GetKeyBytes(Key3TextBox.Text);

                // Use the same IV used during encryption
                byte[] iv = aizat; // Ensure to use the same IV here, possibly store it somewhere

                // Decrypt with Key 1
                byte[] step1 = DecryptDES(cipherText, key3, iv); // Decrypt with Key 3 first

                // Encrypt with Key 2
                byte[] step2 = EncryptDES(step1, key2, iv); // Then encrypt with Key 2

                // Decrypt with Key 1
                byte[] finalResult = DecryptDES(step2, key1, iv); // Finally decrypt with Key 1

                // Convert the final result back to a string
                string decryptedText = Encoding.UTF8.GetString(finalResult);
                DecryptedResultTextBox.Text = decryptedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
