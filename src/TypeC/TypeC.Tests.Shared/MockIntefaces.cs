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
	public interface IWriter
	{
		void Write();
	}
	public interface IInitializer
	{
		void Initialize();
	}
}
