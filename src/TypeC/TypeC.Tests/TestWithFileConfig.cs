/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeC.Tests.Shared;
using TypeC.Tests.Model;

namespace TypeC.Tests
{
	/// <summary>
	/// Summary description for TestLoader
	/// </summary>
	[TestClass]
	public class TestWithFileConfig
	{
		public TestWithFileConfig()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void LoadFromFile()
		{
			string fileName = "TypeConfig.xml";
			TypeContainer tc = TypeContainer.Instance;
			tc.Reset();
			tc.LoadFromFile(fileName);
			IWriter writer1 = tc.GetInstance<IWriter>("writer1");
			IWriter writer2 = tc.GetInstance<IWriter>("writer2");

			Assert.AreNotEqual(writer1, null);
			Assert.AreNotEqual(writer2, null);
		}


		[TestMethod]
		public void TestGenericsWithBuiltInTypes()
		{
			string fileName = "TypeConfig.xml";
			TypeContainer tc = TypeContainer.Instance;
			tc.Reset();
			tc.LoadFromFile(fileName);
			IGenericWriter<string> writer1 = tc.GetInstance<IGenericWriter<string>>("gwriter1");
			IGenericWriter<string> writer2 = tc.GetInstance<IGenericWriter<string>>("gwriter2");
			Assert.AreNotEqual(writer1, null);
			Assert.AreNotEqual(writer2, null);
		}

		[TestMethod]
		public void TestGenericsWithCustomTypes()
		{
			string fileName = "TypeConfig.xml";
			TypeContainer tc = TypeContainer.Instance;
			tc.Reset();
			tc.LoadFromFile(fileName);
			IGenericWriter<MyClass> writer1 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter3");
			writer1.Write(new MyClass());
			IGenericWriter<MyClass> writer2 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter4");
			writer2.Write(new MyClass());
			Assert.AreNotEqual(writer1, null);
			Assert.AreNotEqual(writer2, null);
		}
	}
}
