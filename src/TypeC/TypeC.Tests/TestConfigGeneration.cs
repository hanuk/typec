/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeC.Tests.Model;

namespace TypeC.Tests
{
	[TestClass]
	public class TestConfigGeneration
	{
		[TestMethod]
		public void TestGenericsWithXmlConfig()
		{
			TypeContainer tc = TypeContainer.Instance;
			tc.Reset();
			tc.Register<IDictionary<string, MyClass>, Dictionary<string, MyClass>>();
			tc.Register<IDictionary<string, MyClass>, Dictionary<string, MyClass>>("2nd");

			IDictionary<string, MyClass> d1 = tc.GetInstance<IDictionary<string, MyClass>>();
			IDictionary<string, MyClass> d2 = tc.GetInstance<IDictionary<string, MyClass>>("2nd");
			Assert.AreNotEqual(null, d1);
			Assert.AreNotEqual(null, d2);
			string configXml = tc.GetRegistryAsXml();

			tc.Reset();

			tc.LoadFromString(configXml);
			IDictionary<string, MyClass> d3 = tc.GetInstance<IDictionary<string, MyClass>>();
			IDictionary<string, MyClass> d4 = tc.GetInstance<IDictionary<string, MyClass>>("2nd");
			Assert.AreNotEqual(null, d3);
			Assert.AreNotEqual(null, d4);
		}
	}
}
