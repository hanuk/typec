/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

using System.Diagnostics;
using TypeC.Tests.Shared;

namespace TypeC.Tests
{
	public class MyWriter : IWriter, IInitializer
	{

		#region IWriter Members

		public void Write()
		{
			Debug.WriteLine("In Write()");
		}

		#endregion

		#region IInitializer Members

		public void Initialize()
		{
			Debug.WriteLine("in Initialize()");
		}

		#endregion
	}

	public class NotMyWriter
	{
		//used to test Type.IsAssignableFrom(Type to)
	}
}
