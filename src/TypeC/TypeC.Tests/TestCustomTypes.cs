/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeC.Tests.Lib1;
using TypeC.Tests.Lib2;
using TypeC.Tests.Model;
using TypeC.Tests.Shared;

namespace TypeC.Tests
{
	[TestClass]
	public class TestCustomTypes
	{
		[TestMethod]
		public void TestCustomType()
		{
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IWriter, MyWriter>();
			var writer = typeC.GetInstance<IWriter>();
			Assert.AreNotEqual(null, writer);
			Assert.IsInstanceOfType(writer, typeof(IWriter));
			writer.Write();
		}

		[TestMethod]
		public void TestCustomTypeWithNameSpace()
		{
			string nameSpace = "MyNameSpace";
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IWriter, MyWriter>(nameSpace);
			var writer = typeC.GetInstance<IWriter>(nameSpace);
			Assert.AreNotEqual(null, writer);
			Assert.IsInstanceOfType(writer, typeof(IWriter));
			writer.Write();
		}

		/// <summary>
		/// tests if "to" can be assigned to "from"
		/// </summary>
		[TestMethod]
		public void TestUnAssignableType()
		{
			string nameSpace = "MyNameSpace";
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Reset();
			typeC.Register<IWriter, NotMyWriter>(nameSpace);
			var writer = typeC.GetInstance<IWriter>(nameSpace);
			Assert.AreEqual(null, writer);
		}


        [TestMethod]
        public void TestTypeWithGenericParameterSuccess()
        {
            TypeContainer tc = TypeContainer.Instance;
            tc.Register<IWriter, Lib1Writer1>("writer1");
            tc.Register<IWriter, Lib2Writer1>("writer2");
            tc.Register<IGenericWriter<string>, Lib1GenericWriter1>("gwriter1");
            tc.Register<IGenericWriter<string>, Lib2GenericWriter1>("gwriter2");
            tc.Register<IGenericWriter<MyClass>, Lib1GenericWriter2>("gwriter3");
            tc.Register<IGenericWriter<MyClass>, Lib2GenericWriter2>("gwriter4");
            string xml = tc.GetRegistryAsXml();

            Assert.IsNotNull(tc.GetInstance<IWriter>("writer1"));
            Assert.IsNotNull(tc.GetInstance<IWriter>("writer2"));
            Assert.IsNotNull(tc.GetInstance<IGenericWriter<string>>("gwriter1"));
            Assert.IsNotNull(tc.GetInstance<IGenericWriter<string>>("gwriter2"));
            Assert.IsNotNull(tc.GetInstance<IGenericWriter<MyClass>>("gwriter3"));
            Assert.IsNotNull(tc.GetInstance<IGenericWriter<MyClass>>("gwriter4"));
        }
	}
}
