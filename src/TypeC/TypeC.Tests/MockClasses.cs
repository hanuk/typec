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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeC.Tests
{
	class MyWriter : IWriter, IInitializer
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
}
