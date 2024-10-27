﻿using System;
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
        byte[] combinedKey = new byte[24];//3 keys means 3*8
        public TDES()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyTextBox1.Text != null)
            {
                if (KeyTextBox1.Text.Length == 8 && KeyTextBox2.Text.Length == 8 && KeyTextBox3.Text.Length == 8)
                {
                    string plainText = PlainTextBox.Text;
                    string key1 = KeyTextBox1.Text;
                    string key2 = string.IsNullOrEmpty(KeyTextBox2.Text) ? KeyTextBox1.Text : KeyTextBox2.Text;
                    string key3 = string.IsNullOrEmpty(KeyTextBox3.Text) ? KeyTextBox1.Text : KeyTextBox3.Text;
                    string combined = key1 + key2 + key3;
                    combinedKey = Encoding.UTF8.GetBytes(combined);
                    string ciphertext = EncryptTDES(plainText, combinedKey);
                    EncryptedTextBox.Text = ciphertext;
                    CiphertextBox.Text = ciphertext;
                }
                else {
                    MessageBox.Show("All key must be 8 bytes");
                }
            }
        }
        private string EncryptTDES(string plainText, byte[] combinedKey)
        {
            byte[] dataByte = Encoding.UTF8.GetBytes(plainText);
            using (TripleDESCryptoServiceProvider tdesAlg = new TripleDESCryptoServiceProvider())
            {
                tdesAlg.Key = combinedKey;
                tdesAlg.IV = iv;
                //tdesAlg.Padding = PaddingMode.None;
                ICryptoTransform encryptor = tdesAlg.CreateEncryptor(tdesAlg.Key, tdesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataByte, 0, dataByte.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedData = msEncrypt.ToArray();
                    string showResult = BitConverter.ToString(encryptedData);
                    return showResult;
                }
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CiphertextBox.Text) && !string.IsNullOrEmpty(EncryptedTextBox.Text)) {
                string ciphertext = CiphertextBox.Text;
                string result = ciphertext.Replace("-", "");
                string plaintext = DecryptTDES(result, combinedKey);
                DecryptedTextBox.Text = plaintext;
            }
        }
        private string DecryptTDES(string cipherText, byte[] combinedKey)
        {
            byte[] decrypted;

            byte[] cipherBytes = new byte[cipherText.Length / 2];
            for (int i = 0; i < cipherBytes.Length; i++)
            {
                cipherBytes[i] = Convert.ToByte(cipherText.Substring(i * 2, 2), 16);
            }

            using (TripleDESCryptoServiceProvider tdesAlg = new TripleDESCryptoServiceProvider())
            {
                tdesAlg.Key = combinedKey;
                tdesAlg.IV = iv;
                //tdesAlg.Padding = PaddingMode.None;
                ICryptoTransform decryptor = tdesAlg.CreateDecryptor(tdesAlg.Key, tdesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
                        }
                    }
                }
            }
            return Encoding.UTF8.GetString(decrypted);
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPage AMP = new AdminMainPage("");
            AMP.Show();
            this.Close();
        }
    }
}
