using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Shared;
using TypeC.Tests.Model;

namespace TypeC.Tests.Lib2
{
	public class Lib2GenericConverter: IGenericConverter<MyClass2, MyClass1>
	{
		#region IGenericConverter<MyClass2,MyClass1> Members

		public MyClass2 Convert(MyClass1 input)
		{
			return new MyClass2() { Field = input.Field1 + " : " + input.Field2 };
		}

		#endregion
	}
}
