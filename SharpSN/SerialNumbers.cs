using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SharpSN
{
	/// <summary>
	/// A serial number generator and verifier
	/// </summary>
	public class SerialNumbers
	{
		private readonly int _numberOfSections;
		private readonly int _charactersPerSection;
		private readonly Random _random;
		private readonly char[] _characterPool = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
		private readonly HashAlgorithm _algo;

		/// <summary>
		/// Creates a generator/verifier for a specific key space
		/// </summary>
		/// <param name="numberOfSections">Number of sections/segments (e.g. parts separated by '-') in a valid key</param>
		/// <param name="charactersPerSection">Number of characters in a valid key section/segment</param>
		/// <param name="algo">The hashing algorithm to use (Recommended: SHA256)</param>
		public SerialNumbers(int numberOfSections, int charactersPerSection, HashAlgorithm algo)
		{
			// some requirements must be met
			if (numberOfSections < 2)
			{
				throw new Exception("Number of sections must be at least 2");
			}
			if (algo.HashSize / 8 < charactersPerSection)
			{
				throw new Exception("The number of characters per section cannot exceed the size of "
					+ (_algo.HashSize / 8).ToString() + " for this hash algorithm");
			}
			_numberOfSections = numberOfSections;
			_charactersPerSection = charactersPerSection;
			_algo = algo;
			// initialize random number generator
			_random = new Random();
		}

		/// <summary>
		/// Generate a random, valid serial number
		/// </summary>
		/// <returns>A valid serial number</returns>
		public string GenerateNewSerialNumber()
		{
			var sections = new List<string>();
			while (sections.Count < _numberOfSections - 1)
			{
				var sectionChars = new List<char>();
				// fill the section with up to 
				while (sectionChars.Count < _charactersPerSection)
				{
					var i = _random.Next(0, _characterPool.Length);
					sectionChars.Add(_characterPool[i]);
				}
				// order it randomly so the value of each mapping is not as evident
				var sectionString = new string(sectionChars.ToArray());
				sections.Add(sectionString);
			}
			// all but the last section of the key are entirely random
			var randomPart = String.Join("-", sections.ToArray());
			// the final section is the first N digits of the hash of the first part of the key
			// where N is the number of digits per section
			var hash = ByteArrayToString(_algo.ComputeHash(randomPart.ToCharArray().Select(o => (byte)o).ToArray()));
			var lastPartOfKey = new String(hash.ToCharArray().Take(_charactersPerSection).ToArray());
			sections.Add(lastPartOfKey);
			// join all sections with a "-" and return
			return String.Join("-", sections.ToArray());
		}

		/// <summary>
		/// Verifies if the input serial number is valid
		/// </summary>
		/// <param name="serialNumber">A serial number to validate</param>
		/// <returns>True if it is valid, false otherwise</returns>
		public bool VerifySerialNumber(string serialNumber)
		{
			var sections = serialNumber.Split('-');
			var randomPart = String.Join("-", sections.Take(_numberOfSections - 1));
			var expectedHash = sections.Last();

			// calculate hash of the random part
			var hash = ByteArrayToString(_algo.ComputeHash(randomPart.ToCharArray().Select(o => (byte)o).ToArray()));
			var givenHash = new String(hash.ToCharArray().Take(_charactersPerSection).ToArray());

			return String.Equals(expectedHash, givenHash);
		}

		private static string ByteArrayToString(byte[] ba)
		{
			string hex = BitConverter.ToString(ba);
			return hex.Replace("-", "");
		}
	}
}
