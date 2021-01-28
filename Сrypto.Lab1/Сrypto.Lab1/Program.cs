using System;

namespace Сrypto.Lab1
{
	class Program
	{
		static void Main(string[] args)
		{
			do
			{
				try
				{
					Console.WriteLine("Шифр с использованием кодового слова. Поддерживается только латинский алфавит.\n" +
									"0 - выход\n" +
									"1 - зашифровать сообщение\n" +
									"2 - расшифровать сообщение");
					int n = int.Parse(Console.ReadLine());

					switch (n)
					{
						case 1:
							Console.WriteLine("Введите текст:");
							string sourceWord = Console.ReadLine();

							Console.WriteLine("Введите кодовое слово:");
							string codeword = Console.ReadLine();

							var encryptor = new Encryptor();
							var replacementAlphabet = encryptor.Replace(codeword);
							var encryptedWord = encryptor.EncryptText(replacementAlphabet, sourceWord);
							var str = string.Join("", encryptedWord);
							Console.WriteLine("Зашифрованное сообщение: {0}", str);

							break;
						case 2:
							Console.WriteLine("Введите текст:");
							string sourceWordDec = Console.ReadLine();

							Console.WriteLine("Введите кодовое слово:");
							string codewordDec = Console.ReadLine();

							var decryptor = new Encryptor();
							var replacementAlphabet1 = decryptor.Replace(codewordDec);
							var decryptedWord = decryptor.DecryptText(replacementAlphabet1, sourceWordDec);
							var decStr = string.Join("", decryptedWord);
							Console.WriteLine("Расшифрованное сообщение: {0}", decStr);

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
	}
}