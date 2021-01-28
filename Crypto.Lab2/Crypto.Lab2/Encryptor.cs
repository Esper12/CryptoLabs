using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto.Lab2
{
	class Encryptor
	{
		private uint[] _subKeys;

		// Таблица замен
		private readonly byte[][] _sBox =
		{
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
			  new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF }
		};

		public void SetKey(byte[] key)
		{
			_subKeys = GetSubKeys(key);
		}

		public byte[] Encrypt(byte[] data)
		{
			byte[] reversedData = new byte[data.Length];
			Array.Copy(data, reversedData, data.Length);
			Array.Reverse(reversedData);

			uint leftPart = BitConverter.ToUInt32(reversedData, 0);
			uint rightPart = BitConverter.ToUInt32(reversedData, 4);

			byte[] result = new byte[8];

			for (int i = 0; i < 31; i++)
			{
				int keyIndex;
				if (i < 24)
					keyIndex = i % 8;
				else
					keyIndex = 7 - (i % 8);

				uint round = rightPart ^ funcG(leftPart, _subKeys[keyIndex]);

				rightPart = leftPart;
				leftPart = round;
			}

			rightPart = rightPart ^ funcG(leftPart, _subKeys[0]);

			Array.Copy(BitConverter.GetBytes(leftPart), 0, result, 0, 4);
			Array.Copy(BitConverter.GetBytes(rightPart), 0, result, 4, 4);

			Array.Reverse(result);

			return result;
		}

		public byte[] Decrypt(byte[] data)
		{
			byte[] reversedData = new byte[data.Length];
			Array.Copy(data, reversedData, data.Length);
			Array.Reverse(reversedData);

			uint rightPart = BitConverter.ToUInt32(reversedData, 0);
			uint leftPart = BitConverter.ToUInt32(reversedData, 4);

			byte[] result = new byte[8];

			for (int i = 0; i < 31; i++)
			{
				int keyIndex;
				if (i <= 7)
					keyIndex = i % 8;
				else
					keyIndex = 7 - (i % 8);

				uint round = leftPart ^ funcG(rightPart, _subKeys[keyIndex]);

				leftPart = rightPart;
				rightPart = round;
			}

			leftPart = leftPart ^ funcG(rightPart, _subKeys[0]);

			Array.Copy(BitConverter.GetBytes(rightPart), 0, result, 0, 4);
			Array.Copy(BitConverter.GetBytes(leftPart), 0, result, 4, 4);

			Array.Reverse(result);

			return result;
		}

		// 
		private uint funcG(uint a, uint k)
		{
			uint c = (a + k) % uint.MaxValue;
			uint tmp = Substitute(c);
			return (tmp << 11) | (tmp >> 21);
		}

		// Делаем замены
		private uint Substitute(uint a)
		{
			uint output = 0;

			for (int i = 0; i < 8; i++)
			{
				var temp = (byte)((a >> (4 * i)) & 0x0f);
				temp = _sBox[i][temp];
				output |= (UInt32)temp << (4 * i);
			}

			return output;
		}

		private uint[] GetSubKeys(byte[] key)
		{
			byte[] reversedKey = new byte[key.Length];
			Array.Copy(key, reversedKey, key.Length);
			Array.Reverse(reversedKey);

			uint[] subKeys = new uint[8];
			for (int i = 0; i < 8; i++)
			{
				subKeys[i] = BitConverter.ToUInt32(reversedKey, i * 4);
			}

			Array.Reverse(subKeys);

			return subKeys;
		}

		private static int GetKeyIndex(int i, bool encrypt)
		{
			return encrypt ? (i < 24) ? i % 8 : 7 - (i % 8)
						   : (i < 8) ? i % 8 : 7 - (i % 8);
		}
	}
}
