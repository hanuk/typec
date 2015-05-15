using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using TypeC.Tests.Model;
using TypeC.Tests.Shared;
namespace TypeC.Tests
{
    [TestClass]
    public class TestExternalPath
    {

        [TestMethod]
        public void TestExternalPath_TestGenericsWithBuiltInTypes_NoDependency()
        {
            string fileName = "TypeConfig_External.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.LoadFromFile(fileName);
            IGenericWriter<string> writer1 = tc.GetInstance<IGenericWriter<string>>("gwriter5");
            writer1.Write("test");
            Assert.AreNotEqual(writer1, null);
        }

        [TestMethod]
        public void TestExternalPath_TestGenericsWithCustomTypes_NoDependency()
        {
            string fileName = "TypeConfig_External.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.LoadFromFile(fileName);
            IGenericWriter<MyClass> writer1 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter6");
            writer1.Write(new MyClass());
            Assert.AreNotEqual(writer1, null);
        }

        [TestMethod]
        public void TestExternalPath_TestGenericsWithCustomTypes_Dependency()
        {
            string fileName = "TypeConfig_External_dependency.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();
            tc.LoadFromFile(fileName);
            var obj = tc.GetInstance<Interface1>("interface1");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TestExternalPath_TestGenericsWithCustomTypes_Dependency_XML()
        {
            string fileName = "TypeConfig_External_dependency_NoPath.xml";
            TypeContainer tc = TypeContainer.Instance;
            tc.Reset();

            tc.AddAssemblyProbingPath(@"C:\ws\dev\vso\hkcorp\util\typec\src\TypeC\SharedLibs\");
            tc.LoadFromFile(fileName);

            var obj = tc.GetInstance<Interface1>("interface1");
            Assert.IsNotNull(obj);
        }

        //[TestMethod]
        //public void TestExternalPath_TestGenericsWithCustomTypes_Dependency_XML_Failure_2()
        //{
        //    AppDomain appDomain = AppDomain.CreateDomain("Tester");
        //    var adtHandle = Activator.CreateInstanceFrom(appDomain, "TypeC.Tests.dll", "TypeC.Tests.AppDomainTester");
        //    AppDomainTester adt = (AppDomainTester)adtHandle.Unwrap();
        //    adt.TestExternalPath_TestGenericsWithCustomTypes_Dependency_XML_Failure();
        //    AppDomain.Unload(appDomain);
        //}
    }
}
