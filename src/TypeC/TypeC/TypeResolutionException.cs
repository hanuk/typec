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
