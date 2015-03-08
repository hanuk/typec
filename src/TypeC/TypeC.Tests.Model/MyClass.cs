/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

namespace TypeC.Tests.Model
{
    public class MyClass
    {
		private int _someNumber;
		private string _someString;

		public int SomeNumber
		{
			get { return _someNumber; }
			set { _someNumber = value; }
		}


	public string SomeString
	{
		get { return _someString;}
		set { _someString = value;}
	}
	
		public MyClass()
		{
			this._someString = "Hello";
			this._someNumber = 99;
		}
    }
}
