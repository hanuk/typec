using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Lib6;
using TypeC.Tests.Shared;

namespace TypeC.Tests.Lib5
{
    public class ParentObject:IWriter
    {
        public void Write()
        {
            new ChildObject();
        }
    }
}
