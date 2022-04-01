using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace VisaD.Application.Users.Services
{
    public class PasswordService : IPasswordService
    {
		private readonly int iterCount = 10000;
		private readonly KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256;
		private readonly int numBytesRequested = 256 / 8;

		public string GenerateSalt(int bitCount = 128)
		{
			var salt = new byte[bitCount / 8];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			return Convert.ToBase64String(salt);
		}

		public string HashPassword(string password, string salt)
		{
			var saltBytes = Convert.FromBase64String(salt);

			byte[] subkey = KeyDerivation.Pbkdf2(password, saltBytes, prf, iterCount, numBytesRequested);

			var outputBytes = new byte[8 + subkey.Length];
			WriteNetworkByteOrder(outputBytes, 0, (uint)prf);
			WriteNetworkByteOrder(outputBytes, 4, (uint)iterCount);
			Buffer.BlockCopy(subkey, 0, outputBytes, 8, subkey.Length);

			return Convert.ToBase64String(outputBytes);
		}

		public bool VerifyHashedPassword(string hashedPassword, string providedPassword, string salt)
		{
			if (string.IsNullOrWhiteSpace(hashedPassword) ||
				string.IsNullOrWhiteSpace(providedPassword) ||
				string.IsNullOrWhiteSpace(salt))
			{
				return false;
			}

			byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

			var saltBytes = Convert.FromBase64String(salt);

			try
			{
				// Read header information
				KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 0);
				int iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 4);

				// Read the subkey (the rest of the payload): must be >= 128 bits
				int subkeyLength = decodedHashedPassword.Length - 8;

				byte[] expectedSubkey = new byte[subkeyLength];
				Buffer.BlockCopy(decodedHashedPassword, 8, expectedSubkey, 0, expectedSubkey.Length);

				// Hash the incoming password and verify it
				byte[] actualSubkey = KeyDerivation.Pbkdf2(providedPassword, saltBytes, prf, iterCount, subkeyLength);

				return ByteArraysEqual(actualSubkey, expectedSubkey);
			}
			catch
			{
				// This should never occur except in the case of a malformed payload, where
				// we might go off the end of the array. Regardless, a malformed payload
				// implies verification failed.
				return false;
			}
		}

		private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
		{
			buffer[offset + 0] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)(value >> 0);
		}

		private uint ReadNetworkByteOrder(byte[] buffer, int offset)
		{
			return ((uint)(buffer[offset + 0]) << 24)
					| ((uint)(buffer[offset + 1]) << 16)
					| ((uint)(buffer[offset + 2]) << 8)
					| ((uint)(buffer[offset + 3]));
		}

		// Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null || b == null || a.Length != b.Length)
			{
				return false;
			}
			var areSame = true;
			for (var i = 0; i < a.Length; i++)
			{
				areSame &= (a[i] == b[i]);
			}
			return areSame;
		}
	}
}
