/*  
 Copyright (c) Microsoft.  All rights reserved.  
  
Licensed under the Apache License, Version 2.0 (the "License");  
you may not use this file except in compliance with the License.  
You may obtain a copy of the License at //   http://www.apache.org/licenses/LICENSE-2.0  
  
Unless required by applicable law or agreed to in writing, software  
distributed under the License is distributed on an "AS IS" BASIS,  
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
*/

using System.Diagnostics;
/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
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
