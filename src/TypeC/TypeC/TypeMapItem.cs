/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeC
{
	public class TypeMapItem
	{
		public string Namespace { get; set; }
		public string FromTypeName { get; set; }
		public string ToTypeName { get; set; }
	}
}
