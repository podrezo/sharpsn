using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SharpSN;

namespace SNGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			string inputNumberSections = null;
			string inputCharsPerSection = null;
			string inputNumToGenerate = null;
			uint numberOfSections;
			uint charsPerSection;
			uint numberOfCodesToGenerate;
			Console.WriteLine("Serial Number Generator by Petro Podrezo");
			while (!uint.TryParse(inputNumberSections, out numberOfSections))
			{
				Console.Write("Input # of sections in key [5]: ");
				inputNumberSections = Console.ReadLine();
				if (String.IsNullOrEmpty(inputNumberSections))
				{
					numberOfSections = 5;
					break;
				}
			}
			while (!uint.TryParse(inputCharsPerSection, out charsPerSection))
			{
				Console.Write("Input # of characters per section [5]: ");
				inputCharsPerSection = Console.ReadLine();
				if (String.IsNullOrEmpty(inputCharsPerSection))
				{
					charsPerSection = 5;
					break;
				}
			}
			while (!uint.TryParse(inputNumToGenerate, out numberOfCodesToGenerate))
			{
				Console.Write("How many codes do you want to generate? [10] ");
				inputNumToGenerate = Console.ReadLine();
				if (String.IsNullOrEmpty(inputCharsPerSection))
				{
					numberOfCodesToGenerate = 10;
					break;
				}
			}

			SerialNumbers generator;
			try
			{
				generator = new SerialNumbers((int)numberOfSections, (int)charsPerSection, SHA256.Create());
				// Generate however many codes are required
				for (int i = 0; i < numberOfCodesToGenerate; i++)
				{
					Console.WriteLine(generator.GenerateNewSerialNumber());
				}
				// Test codes phase
				Console.WriteLine("Input code to test and hit enter; press Ctrl+C to exit");
				string inputCode = null;
				while (true)
				{
					Console.Write("sn> ");
					inputCode = Console.ReadLine();
					var valid = generator.VerifySerialNumber(inputCode);
					Console.WriteLine(String.Format("{0}: {1}", valid ? "VALID" : "INVALID", inputCode));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem with your input:");
				Console.WriteLine(e.Message);
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
		}
	}
}
