using System;
using System.Security.Cryptography;
using System.Text;

namespace VisaD.Application.Utils
{
	public class RandomStringGenerator
	{
		public RandomStringGenerator(int stringLength)
		{
			this.StringLength = stringLength;
		}

		public RandomStringGenerator(int stringLength, string characters)
		{
			this.Characters = characters;
			this.StringLength = stringLength;
		}

		public RandomStringGenerator(int stringLength, bool hasRepetitiveCharacters, string characters)
		{
			this.Characters = characters;
			this.HasRepetitiveCharacters = hasRepetitiveCharacters;
			this.StringLength = stringLength;
		}

		public bool HasRepetitiveCharacters { get; set; } = false;
		public int StringLength
		{
			get
			{
				return this._stringLength;
			}
			set
			{
				if (!this.HasRepetitiveCharacters && value > this.Characters.Length)
				{
					throw new ArgumentOutOfRangeException($"{nameof(StringLength)}", $"The value of {nameof(StringLength)} cannot be greater than the length of the used characters when no repetitive characters are allowed!");
				}

				this._stringLength = value;
			}
		}
		public string Characters
		{
			get
			{
				return new string(this._characters);
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					this._characters = value.ToCharArray();
				}
			}
		}

		public string Generate()
		{
			using (var generator = RandomNumberGenerator.Create())
			{
				var result = new StringBuilder(this.StringLength);
				for (int i = 0; i <= this.StringLength - 1; i++)
				{
					char ch;
					do
					{
						int randomIndex = this.GetCryptographicRandomNumber(generator, this._characters.GetLowerBound(0), this._characters.GetUpperBound(0));
						ch = this._characters[randomIndex];
					}
					while (!this.HasRepetitiveCharacters && result.ToString().IndexOf(ch) >= 0);

					result.Append(ch);
				}

				return result.ToString();
			}
		}

		private int GetCryptographicRandomNumber(RandomNumberGenerator generator, int fromInclusive, int toExclusive)
		{
			if (fromInclusive == toExclusive - 1)
			{
				return fromInclusive;
			}

			var randomNumber = new byte[sizeof(int)];
			generator.GetNonZeroBytes(randomNumber);
			var value = BitConverter.ToInt32(randomNumber, 0);

			// constrain value between from and to
			var result = ((value - fromInclusive) % (toExclusive - fromInclusive + 1) + (toExclusive - fromInclusive + 1)) % (toExclusive - fromInclusive + 1) + fromInclusive;
			return result;
		}

		private char[] _characters = ("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789").ToCharArray();
		private int _stringLength = 10;
	}
}
