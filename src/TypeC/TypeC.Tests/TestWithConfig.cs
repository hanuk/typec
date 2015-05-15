/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeC.Tests.Shared;
using TypeC.Tests.Model;
using System;

namespace TypeC.Tests
{
	/// <summary>
	/// Summary description for TestLoader
	/// </summary>
	[TestClass]
	public class TestWithConfig
	{
		public TestWithConfig()
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
		public void Config_LoadFromFile()
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
		public void Config_TestGenericsWithBuiltInTypes()
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
		public void Config_TestGenericsWithCustomTypes()
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

		[TestMethod]
		public void Config_TestGenericsWithNestedCustomTypes()
		{
			TypeContainer tc = TypeContainer.Instance;
			tc.Register<IGenericConverter<MyClass2, MyClass1>, TypeC.Tests.Lib1.Lib1GenericConverter>();
			IGenericConverter<MyClass2, MyClass1> gc = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>();
			Assert.AreNotEqual(null, gc);
			MyClass2 mc = gc.Convert(new MyClass1() { Field1 = "field1", Field2 = "field2" });
		}

		[TestMethod]
		public void Config_TestGenericsWithNestedCustomTypesFromConfig()
		{
			string fileName = "TypeConfig.xml";
			TypeContainer tc = TypeContainer.Instance;
			tc.Reset();
			tc.LoadFromFile(fileName);
			IGenericConverter<MyClass2, MyClass1> gc = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>();
			Assert.AreNotEqual(null, gc);
			MyClass2 mc = gc.Convert(new MyClass1() { Field1 = "field1", Field2 = "field2" });
			Assert.AreEqual(mc.Field, "field1" + " : " + "field2");
		}

		[TestMethod]
		public void Config_TestGenericsWithNestedCustomTypesAndInlineXml()
		{
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.Register<IGenericConverter<MyClass2, MyClass1>, TypeC.Tests.Lib1.Lib1GenericConverter>();
            tc.Register<IGenericConverter<MyClass2, MyClass1>, TypeC.Tests.Lib2.Lib2GenericConverter>("Lib2");
            IGenericConverter<MyClass2, MyClass1> gc1 = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>();
            IGenericConverter<MyClass2, MyClass1> gc2 = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>("Lib2");
            Assert.AreNotEqual(null, gc1);
            Assert.AreNotEqual(null, gc2);

            string xml = tc.GetRegistryAsXml();
            tc.Reset();
            tc.LoadFromString(xml);
            IGenericConverter<MyClass2, MyClass1> gc3 = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>();
            IGenericConverter<MyClass2, MyClass1> gc4 = tc.GetInstance<IGenericConverter<MyClass2, MyClass1>>("Lib2");
            Assert.AreNotEqual(null, gc3);
            Assert.AreNotEqual(null, gc4);
		}
		[TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
		public void Config_TestInvalidFromType()
		{
			//"from" doesn't specify the assembly name
			string xml = "<mapping namespace=\"DEFAULT\" from=\"TypeC.Tests.Shared.IGenericConverter`2[[TypeC.Tests.Model.MyClass2, TypeC.Tests.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[TypeC.Tests.Model.MyClass1, TypeC.Tests.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]\" to=\"TypeC.Tests.Lib1.Lib1GenericConverter, TypeC.Tests.Lib1\"/>";
			TypeContainer tc = TypeContainer.Instance;
			tc.LoadFromString(xml);
            //Assert.AreNotEqual(0, tc.Errors.Length);
		}

		[TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
		public void Config_TestInvalidToType()
		{
			//"to" doesn't specify the assembly name
			string xml = "<mapping namespace=\"DEFAULT\" from=\"TypeC.Tests.Shared.IGenericConverter`2[[TypeC.Tests.Model.MyClass2, TypeC.Tests.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[TypeC.Tests.Model.MyClass1, TypeC.Tests.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], TypeC.Tests.Shared\" to=\"TypeC.Tests.Lib1.Lib1GenericConverter\"/>";
			TypeContainer tc = TypeContainer.Instance;
			tc.LoadFromString(xml);
            Assert.AreNotEqual(0, tc.Errors.Length);
		}
	}
}
