using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib3
{
    public class DependentClass:Interface1
    {
        public DependentClass()
        {
            TypeC.Tests.Lib4.Class1 cls1 = new TypeC.Tests.Lib4.Class1(); 
        }
    }
}
