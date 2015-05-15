using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Lib7;

namespace TypeC.Tests.Lib6
{
    public class ChildObject
    {
        public ChildObject()
        {
            new GrandChild();
        }

    }
}
