﻿/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Model;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib2
{
	public class Lib2GenericWriter2:IGenericWriter<MyClass>
	{
		#region IGenericWriter<MyClass> Members

		public void Write(MyClass msg)
		{
			Debug.WriteLine("in IGenericWriter<MyClass> : " + msg.SomeNumber);
			Debug.WriteLine("in IGenericWriter<MyClass> : " + msg.SomeString);
		}

		#endregion
	}
}
