﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ClientWindows.Core.Encryption
{
    public class AES
    {
        private const int IVLENGTH = 16;
        private static byte[] _key;

        public static void PreHashKey(string key)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                _key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        public static string Encrypt(string input,string key)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input),Encoding.UTF8.GetBytes(key)));
        }
        public static string Encrypt(string input)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input)));
        }
        public static byte[] Encrypt(byte[] input)
        {
            if (_key == null || _key.Length == 0) throw new Exception("Key can not be empty.");
            if (input == null || input.Length == 0) throw new ArgumentException("Input can not be empty.");

            byte[] data = input, encdata = new byte[0];

            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var rd = new RijndaelManaged { Key = _key })
                    {
                        rd.GenerateIV();

                        using (var cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            ms.Write(rd.IV, 0, rd.IV.Length); // write first 16 bytes IV, followed by encrypted message
                            cs.Write(data, 0, data.Length);
                        }
                    }

                    encdata = ms.ToArray();
                }
            }
            catch
            {
            }
            return encdata;
        }
        public static byte[] Encrypt(byte[] input, byte[] key)
        {
            if (key == null || key.Length == 0) throw new Exception("Key can not be empty.");
            if (input == null || input.Length == 0) throw new ArgumentException("Input can not be empty.");

            using (var md5 = new MD5CryptoServiceProvider())
            {
                key = md5.ComputeHash(key);
            }

            byte[] data = input, encdata = new byte[0];

            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var rd = new RijndaelManaged { Key = key })
                    {
                        rd.GenerateIV();

                        using (var cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            ms.Write(rd.IV, 0, rd.IV.Length); // write first 16 bytes IV, followed by encrypted message
                            cs.Write(data, 0, data.Length);
                        }
                    }

                    encdata = ms.ToArray();
                }
            }
            catch
            {
            }
            return encdata;
        }        

        public static string Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input)));
        }

        public static byte[] Decrypt(byte[] input)
        {
            if (_key == null || _key.Length == 0) throw new Exception("Key can not be empty.");
            if (input == null || input.Length == 0) throw new ArgumentException("Input can not be empty.");

            byte[] data = new byte[0];

            try
            {
                using (var ms = new MemoryStream(input))
                {
                    using (var rd = new RijndaelManaged { Key = _key })
                    {
                        byte[] iv = new byte[IVLENGTH];
                        ms.Read(iv, 0, IVLENGTH); // read first 16 bytes for IV, followed by encrypted message
                        rd.IV = iv;

                        using (var cs = new CryptoStream(ms, rd.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] temp = new byte[ms.Length - IVLENGTH + 1];
                            data = new byte[cs.Read(temp, 0, temp.Length)];
                            Buffer.BlockCopy(temp, 0, data, 0, data.Length);
                        }
                    }
                }
            }
            catch
            {
            }
            return data;
        }
    }
}
