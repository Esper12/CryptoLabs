using Crypto.Lab2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Сrypto.Lab1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.Unicode;
			Console.InputEncoding = Encoding.Unicode;

			do
			{
				try
				{
					Console.WriteLine("Шифр ГОСТ 28147-89.\n" +
									"0 - выход\n" +
									"1 - зашифровать сообщение\n" +
									"2 - расшифровать сообщение");
					int n = int.Parse(Console.ReadLine());

					switch (n)
					{
						case 1:
							Console.WriteLine("Введите текст:");
							string sourceWord = Console.ReadLine();
							Console.WriteLine("Введите ключ:");
							string key = Console.ReadLine();

							var result = Encrypt(sourceWord, key);
							Console.WriteLine(result);

							break;
						case 2:
							Console.WriteLine("Введите текст:");
							sourceWord = Console.ReadLine();
							Console.WriteLine("Введите ключ:");
							key = Console.ReadLine();

							result = Decrypt(sourceWord, key);
							Console.WriteLine(result);

							break;
						case 0:
							return;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Ошибка: {e.Message}");
				}
				finally
				{
					Console.WriteLine();
				}
			}
			while (true);
		}

		static string Encrypt(string text, string key)
		{
			var textToBytes = Encoding.Unicode.GetBytes(text);
			var keyToBytes = Encoding.Unicode.GetBytes(key);

			return Encrypt(textToBytes, keyToBytes);
		}

		static string Encrypt(byte[] data, byte[] key)
		{
			var encryptor = new Encryptor();
			encryptor.SetKey(key);

			var blocks = Split(data);

			var result = new byte[data.Length];
			for (int i = 0; i < blocks.Length; i++)
			{
				var encryptedBlock = encryptor.Encrypt(blocks[i]);

				Array.Copy(encryptedBlock, 0, result, i * 8, 8);
			}

			return Encoding.Unicode.GetString(result);
		}

		static string Decrypt(string text, string key)
		{
			var textToBytes = Encoding.Unicode.GetBytes(text);
			var keyToBytes = Encoding.Unicode.GetBytes(key);
			var encryptor = new Encryptor();
			encryptor.SetKey(keyToBytes);

			var blocks = Split(textToBytes);

			var result = new byte[textToBytes.Length];
			for (int i = 0; i < blocks.Length; i++)
			{
				var encryptedBlock = encryptor.Decrypt(blocks[i]);

				Array.Copy(encryptedBlock, 0, result, i * 8, 8);
			}

			return Encoding.Unicode.GetString(result);
		}

		static byte[][] Split(byte[] data, int blockSize = 8)
		{
			var blocksCount = data.Length / blockSize;
			var result = new List<byte[]>(); ;
			for (int i = 0; i < blocksCount; i++)
			{
				byte[] block = new byte[blockSize];
				Array.Copy(data, i * blockSize, block, 0, blockSize);
				result.Add(block);
			}

			return result.ToArray();
		}
	}
}