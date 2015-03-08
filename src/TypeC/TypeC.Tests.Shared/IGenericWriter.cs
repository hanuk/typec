/*
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information
*/

namespace TypeC.Tests.Shared
{
	public interface IGenericWriter<T>
	{
		void Write(T msg);
	}
}
