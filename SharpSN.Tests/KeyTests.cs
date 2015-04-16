using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpSN.Tests
{
	[TestClass]
	public class KeyTests
	{
		private SerialNumbers generator;

		[TestInitialize]
		public void Setup()
		{
			generator = new SerialNumbers(5, 6, SHA256.Create());
		}

		[TestMethod]
		public void Generator_BasicTest()
		{
			var validKey = generator.GenerateNewSerialNumber();
			Assert.IsNotNull(validKey);
			Assert.AreNotEqual<string>(String.Empty, validKey);
			Assert.IsTrue(generator.VerifySerialNumber(validKey), "Generated valid key (" + validKey + ") is not valid");
		}

		[TestMethod]
		/* This test is not... fantastic... but it is a basic way to check
		 * with some degree of confidence that we don't generate duplicate
		 * keys TOO frequently */
		public void Generator_NoCollisionSanityCheck()
		{
			var keys = new List<string>();
			for (int i = 0; i < 1000; i++)
			{
				keys.Add(generator.GenerateNewSerialNumber());
			}
			keys = keys.OrderBy(o => o).ToList();

			for (int j = 0; j < 999; j++)
			{
				Assert.AreNotEqual<string>(keys[j], keys[j + 1], "Collision at index " + j.ToString());
			}
		}

		[TestMethod]
		public void Verifier_Good_WellKnownKey()
		{
			Assert.IsTrue(generator.VerifySerialNumber("23ADD2-88A748-D41235-ECEB24-503208"));
		}

		[TestMethod]
		public void Verifier_Bad_WellKnownKey()
		{
			// changed one character from the good key
			Assert.IsFalse(generator.VerifySerialNumber("23ADD2-87A748-D41235-ECEB24-503208"));
		}

		[TestMethod]
		[Timeout(1000)]
		// Ensure 1000 keys can be generated in less than a second
		public void Generator_OptimizationTest()
		{
			for (int i = 0; i < 1000; i++)
			{
				generator.GenerateNewSerialNumber();
			}
		}
	}
}
