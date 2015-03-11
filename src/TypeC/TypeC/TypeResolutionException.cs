/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeC
{
	public class TypeResolutionException:ApplicationException
	{
		public TypeResolutionException(): base(){}
		public TypeResolutionException(string message) :base(message) {	}
		public TypeResolutionException(string message, Exception inner) : base(message, inner) { }
	}
}
