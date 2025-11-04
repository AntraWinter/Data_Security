using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace WpfApp1
{
	public static class CryptoHelper
	{
		// Фиксированная парольная фраза
		public const string CorrectPhrase = "Saturn";

		private static byte[] GenerateKeyWithMD2(string password, byte[] salt)
		{
			var md2Digest = new MD2Digest();
			var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
			var combined = new byte[passwordBytes.Length + salt.Length];

			Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
			Buffer.BlockCopy(salt, 0, combined, passwordBytes.Length, salt.Length);

			md2Digest.BlockUpdate(combined, 0, combined.Length);
			var keyMaterial = new byte[24]; // 16 байт для ключа + 8 байт для IV
			md2Digest.DoFinal(keyMaterial, 0);

			return keyMaterial;
		}



		// Шифрование файла
		public static void EncryptFile(string inputFile, string outputFile, string password)
		{
			try
			{
				byte[] salt = new byte[8];
				using (var rng = new RNGCryptoServiceProvider())
				{
					rng.GetBytes(salt);
				}

				byte[] keyMaterial = GenerateKeyWithMD2(password, salt);
				byte[] key = new byte[16]; // 128-битный ключ
				byte[] iv = new byte[8];  // 64-битный IV для RC2

				Buffer.BlockCopy(keyMaterial, 0, key, 0, key.Length);
				Buffer.BlockCopy(keyMaterial, key.Length, iv, 0, iv.Length);

				using (RC2 rc2 = RC2.Create())
				{
					rc2.Key = key;
					rc2.IV = iv;
					rc2.Mode = CipherMode.CBC;

					using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
					{
						fsOutput.Write(salt, 0, salt.Length);

						using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
						using (CryptoStream cryptoStream = new CryptoStream(fsOutput, rc2.CreateEncryptor(), CryptoStreamMode.Write))
						{
							fsInput.CopyTo(cryptoStream);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при шифровании файла: {ex.Message}");
			}
		}


		// Расшифрование файла
		public static void DecryptFile(string inputFile, string outputFile, string password)
		{
			try
			{
				byte[] salt = new byte[8];

				using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
				{
					fsInput.Read(salt, 0, salt.Length);

					byte[] keyMaterial = GenerateKeyWithMD2(password, salt);
					byte[] key = new byte[16]; // 128-битный ключ
					byte[] iv = new byte[8];  // 64-битный IV для RC2

					Buffer.BlockCopy(keyMaterial, 0, key, 0, key.Length);
					Buffer.BlockCopy(keyMaterial, key.Length, iv, 0, iv.Length);

					using (RC2 rc2 = RC2.Create())
					{
						rc2.Key = key;
						rc2.IV = iv;
						rc2.Mode = CipherMode.CBC;

						using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
						using (CryptoStream cryptoStream = new CryptoStream(fsInput, rc2.CreateDecryptor(), CryptoStreamMode.Read))
						{
							cryptoStream.CopyTo(fsOutput);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при расшифровании файла: {ex.Message}");
			}
		}

	}
}
