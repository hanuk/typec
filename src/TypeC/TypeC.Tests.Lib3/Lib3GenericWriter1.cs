/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System.Diagnostics;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib3
{
	public class Lib3GenericWriter1:IGenericWriter<string>
	{
		#region IGenericWriter<string> Members

		public void Write(string msg)
		{
			Debug.WriteLine("in Lib3GenericWriter1.Write: " + msg); 
		}

		#endregion
	}
}
