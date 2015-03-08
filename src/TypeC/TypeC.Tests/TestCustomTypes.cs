/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	}
}
