using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APSWCWEBAPIAPP.Services
{

	public class EncDecrpt
	{
		private static string key_value = "4512631236589784";
		private static string iv_value = "4512631236589784";

		public static string Encrypt_Data(string data)
		{
			var keybytes = Encoding.UTF8.GetBytes(key_value);
			var iv = Encoding.UTF8.GetBytes(iv_value);
			var encryptStringToBytes = EncryptStringToBytes(data, keybytes, iv);
			return Convert.ToBase64String(encryptStringToBytes);
		}

		public static string Decrypt_Data(string data)
		{
			var enc_value = Convert.FromBase64String(data);
			var keybytes = Encoding.UTF8.GetBytes(key_value);
			var iv = Encoding.UTF8.GetBytes(iv_value);
			var roundtrip = DecryptStringFromBytes(enc_value, keybytes, iv);
			return roundtrip;


		}

		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			byte[] encrypted;
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;
				rijAlg.Key = key;
				rijAlg.IV = iv;
				var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			return encrypted;
		}


		private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
		{
			if (cipherText == null || cipherText.Length <= 0)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			string plaintext = null;
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;
				rijAlg.Key = key;
				rijAlg.IV = iv;
				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
				try
				{
					using (var msDecrypt = new MemoryStream(cipherText))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (var srDecrypt = new StreamReader(csDecrypt))
							{
								plaintext = srDecrypt.ReadToEnd();
							}
						}
					}
				}
				catch (Exception ex)
				{
					plaintext = ex.Message;
					throw ex;

				}

			}

			return plaintext;
		}

	}
}