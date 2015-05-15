using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeC.Tests.Shared;

namespace TypeC.Tests
{
    [TestClass]
    public class TestExternalPath2
    {
        /// <summary>
        /// Tests for TypeResolutionException when no 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void TestExternalPath2_TestGenericsWithCustomTypes_Dependency_XML_Failure()
        {
            string fileName = "TypeConfig_External_dependency_NoPath.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();

            tc.LoadFromFile(fileName);
            //var obj = tc.GetInstance<Interface1>("interface1");
            //Assert.AreEqual(1, tc.Errors.Length);
            //Assert.IsNull(obj);
        }
        [TestMethod]
        public void TestExternalPath2_TestGenericsWithCustomTypes_Dependency_XML_Failure_2()
        {
            string fileName = "TypeConfig_External_dependency_NoPath.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            try
            {
                tc.LoadFromFile(fileName);
            }
            catch(TypeLoadException te)
            {

            }
            Assert.AreEqual(1, tc.Errors.Length);
            var obj = tc.GetInstance<Interface1>("interface1");
            Assert.IsNull(obj);
        }

        /// <summary>
        /// This fails due to  the tightly coupled assebly in SharedLib3 can't be found as the path is not specified.
        /// Order of tests makes difference due to AppDomain assembly loaded state. So, don't chnage the order of TestExternalPath2_TypesInTwoExternalDirectories_Failure 
        /// and TestExternalPath2_TypesInTwoExternalDirectories_Success
        /// In future, run each of these tests in their own AppDomain to avoid this problem
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestExternalPath2_TypesInTwoExternalDirectories_Failure()
        {
            string fileName = "TypeConfig_External_dependency_multipath2.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.LoadFromFile(fileName);
            IWriter writer = tc.GetInstance<IWriter>();
            Assert.IsNotNull(writer);
            writer.Write();
        }

        /// <summary>
        /// Tests with base type in shared directory, child in SharedLib2, grand child in SharedLib3
        /// </summary>
        [TestMethod]
        public void TestExternalPath2_TypesInTwoExternalDirectories_Success()
        {
            string fileName = "TypeConfig_External_dependency_multipath.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.LoadFromFile(fileName);
            IWriter writer = tc.GetInstance<IWriter>();
            Assert.IsNotNull(writer);
            writer.Write();
        }
    }
}
