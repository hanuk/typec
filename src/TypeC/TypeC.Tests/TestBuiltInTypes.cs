/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeC.Tests
{
	[TestClass]
	public class TestBuiltInTypes
	{
		[TestMethod]
		public void TestDictionary()
		{
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IDictionary<string, string>, Dictionary<string, string>>();
			var dict =  typeC.GetInstance<IDictionary<string, string>>();
			Assert.AreNotEqual(null, dict);
			dict.Add("test", "testValue");
			var val = dict["test"];
			Assert.AreEqual("testValue", val);
		}

		[TestMethod]
		public void TestList()
		{
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IList<string>, List<string>>();
			var list = typeC.GetInstance<IList<string>>();
			Assert.AreNotEqual(null, list);
			list.Add("test");
			var item = list[0];
			Assert.AreEqual("test", item);
		}

		[TestMethod]
		public void TestDictionaryWithNamespace()
		{
			string nameSpace = "MyNameSpace";
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IDictionary<string, string>, Dictionary<string, string>>(nameSpace);
			var dict = typeC.GetInstance<IDictionary<string, string>>(nameSpace);
			Assert.AreNotEqual(null, dict);
			dict.Add("test", "testValue");
			var val = dict["test"];
			Assert.AreEqual("testValue", val);
		}

		[TestMethod]
		public void TestListWithNamespace()
		{
			string nameSpace = "MyNameSpace";
			TypeContainer typeC = TypeContainer.Instance;
			typeC.Register<IList<string>, List<string>>(nameSpace);
			var list = typeC.GetInstance<IList<string>>(nameSpace);
			Assert.AreNotEqual(null, list);
			list.Add("test");
			var item = list[0];
			Assert.AreEqual("test", item);
		}
	}
}
