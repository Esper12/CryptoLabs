using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Сrypto.Lab1
{
	public class Encryptor
	{
		public char[] alphabet = { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };

		public char[] Replace(string codeword)
		{
			char[] array = codeword.ToCharArray();
			char[] result = array.Concat(alphabet).Distinct().ToArray();

			return result;
		}

		public char[] EncryptText(char[] replacementAlphabet, string sourceMessage)
		{
			char[] result;
			char[] array = sourceMessage.ToCharArray();

			var replacementDict = new Dictionary<char, char>();
			for(int i = 0; i < alphabet.Length; i++)
			{
				replacementDict.Add(alphabet[i], replacementAlphabet[i]);
			}

			result = sourceMessage.Select((c) => {
				var isFounded = replacementDict.TryGetValue(c, out var value);
				return isFounded ? value : c;
			}).ToArray();

			return result;
		}

		public char[] DecryptText(char[] replacementAlphabet, string sourceMessage)
		{
			char[] result;
			char[] array = sourceMessage.ToCharArray();

			var replacementDict = new Dictionary<char, char>();
			for (int i = 0; i < alphabet.Length; i++)
			{
				replacementDict.Add(replacementAlphabet[i], alphabet[i]);
			}

			result = sourceMessage.Select((c) => {
				var isFounded = replacementDict.TryGetValue(c, out var value);
				return isFounded ? value : c;
			}).ToArray();

			return result;
		}
	}
}
