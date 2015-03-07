/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib1
{
	public class Lib1Writer1 : IWriter, IInitializer
	{

		#region IWriter Members

		public void Write()
		{
			Debug.WriteLine("In Lib1Writer1.Write()");
		}

		#endregion

		#region IInitializer Members

		public void Initialize()
		{
			Debug.WriteLine("in Lib1Writer1.Initialize()");
		}

		#endregion
	}

	public class Lib1Writer2 : IWriter, IInitializer
	{

		#region IWriter Members

		public void Write()
		{
			Debug.WriteLine("In Lib1Writer2.Write()");
		}

		#endregion

		#region IInitializer Members

		public void Initialize()
		{
			Debug.WriteLine("in Lib1Writer2.Initialize()");
		}

		#endregion
	}
}
