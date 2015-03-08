/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System.Diagnostics;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib1
{
	public class Lib1GenericWriter1:IGenericWriter<string>
	{
		#region IGenericWriter<string> Members

		public void Write(string msg)
		{
			Debug.WriteLine("in Lib1GenericWriter1.Write: " + msg); 
		}

		#endregion
	}
}
