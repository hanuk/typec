/*  
 Copyright (c) Microsoft.  All rights reserved.  
  
Licensed under the Apache License, Version 2.0 (the "License");  
you may not use this file except in compliance with the License.  
You may obtain a copy of the License at //   http://www.apache.org/licenses/LICENSE-2.0  
  
Unless required by applicable law or agreed to in writing, software  
distributed under the License is distributed on an "AS IS" BASIS,  
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
	}
}
