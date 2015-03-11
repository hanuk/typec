/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeC.Tests.Shared
{
	public interface IGenericConverter<T1, T2>
	{
		T1 Convert(T2 input); 
	}
}
