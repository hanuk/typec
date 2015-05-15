####Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information.####

#TypeC - A Simple .NET Type Container#
###Version 0.4.0###
TypeC is a simple .NET type container for resolving interfaces/abstract base classes to their concrete implementations. TypeC implements factory method for instantiating types. Only one container can exist in a given .NET AppDomain as it is a singleton implementation. Multiple concrete child types can be mapped to the same base class through namespace support.

The project repo is located at [http://github.com/hanuk/typec](http://github.com/hanuk/typec); the nuget package can be installed using the command: "Install-Package TypeC" from the NuGet Package Manager Console inside Visual Studio.

The following snippets show sample usage:

	//during the startup 
	 TypeContainer typeC = TypeContainer.Instance; 
	 typeC.Register<IList<string>, List<string>>(); 
	
	 //acquire the implementation
	 var list = typeC.GetInstance<IList<string>>(); 
	 list.Add("test"); 

With namespace support:
	
	//during the startup 
	string namespace = "MyNamespace"; 
	TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>(namespace);

	//acquire the implementation
	var list = typeC.GetInstance<IList<string>>(namespace);
	list.Add("test"); 

Loading type mapping from a configuration file:

	//during the startup
	string fileName = "TypeConfig.xml";
	TypeContainer tc = TypeContainer.Instance;
	tc.Reset();
	//the Load(fileName) will fail if types can't be resolved 
	tc.Load(fileName);
	

	//acquire the implementation
	IGenericWriter<MyClass> writer1 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter3");
	writer1.Write(new MyClass());
	IGenericWriter<MyClass> writer2 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter4");

Sample XML configuration file is located in TypeC.Tests project. The file follows the standard reflection notation: 
[https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx "Text Notation for Describing .NET Type")

TypeContainer.GetRegistryAsXml() will dump the registry for easy configuration file creation and debugging. The returned XML string can be pasted into the configuration file so that complex generic types can be defined easily for dynamic binding.

The sample xml configuration snippet returned by TypeContainer.GetRegistryAsXml()is shown below:

	//code snippet 
	TypeContainer tc = TypeContainer.Instance;
	tc.Register<IWriter, Lib1Writer1>("writer1");
	tc.Register<IWriter, Lib2Writer1>("writer2");
	tc.Register<IGenericWriter<string>, Lib1GenericWriter1>("gwriter1");
	tc.Register<IGenericWriter<string>, Lib2GenericWriter1>("gwriter2");
	tc.Register<IGenericWriter<MyClass>, Lib1GenericWriter2>("gwriter3");
	tc.Register<IGenericWriter<MyClass>, Lib2GenericWriter2>("gwriter4");
	 
	string xml = tc.GetRegistryAsXml();

The corresponding generated XML looks as below:

	<typecmap>
	 <mapping namespace="writer1" from="TypeC.Tests.Shared.IWriter,TypeC.Tests.Shared" to="TypeC.Tests.Lib1.Lib1Writer1, TypeC.Tests.Lib1"/>
	 <mapping namespace="writer2" from="TypeC.Tests.Shared.IWriter,TypeC.Tests.Shared" to="TypeC.Tests.Lib1.Lib1Writer2, TypeC.Tests.Lib1"/> 
	 <mapping namespace="gwriter1" from="TypeC.Tests.Shared.IGenericWriter`1[[System.String, mscorlib]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib1.Lib1GenericWriter1, TypeC.Tests.Lib1"/> 
	 <mapping namespace="gwriter2" from="TypeC.Tests.Shared.IGenericWriter`1[[System.String, mscorlib]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib2.Lib2GenericWriter1, TypeC.Tests.Lib2"/> 
	 <mapping namespace="gwriter3" from="TypeC.Tests.Shared.IGenericWriter`1[[TypeC.Tests.Model.MyClass, TypeC.Tests.Model]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib1.Lib1GenericWriter2, TypeC.Tests.Lib1"/> 
	 <mapping namespace="gwriter4" from="TypeC.Tests.Shared.IGenericWriter`1[[TypeC.Tests.Model.MyClass, TypeC.Tests.Model]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib2.Lib2GenericWriter2, TypeC.Tests.Lib2"/> 
	</typecmap> 

If you have complex nested generic types, hand coding XML can be error prone; build your registry using code and dump xml into the configuration file.

###Version 0.5.0 ###
In version 0.5.0, assemblies from external directories can be loaded into the container. 
Probing paths can be added to the container using the code snippet:
	
	TypeContainer tc = TypeContainer.Instance;
	tc.AddAssemblyProbingPath(@"C:\ws\dev\vso\hkcorp\util\typec\src\TypeC\SharedLibs\");
	tc.AddAssemblyProbingPath(@"C:\ws\dev\vso\hkcorp\util\typec\src\TypeC\SharedLibs2\");
	//register a few types
	string xml = tc.GetRegistryAsXml();

The corresponding generated XML looks as below:

	<typecmap>
	 <probinglibpath path="C:\ws\dev\vso\hkcorp\util\typec\src\TypeC\SharedLibs\"/>
	 <probinglibpath path="C:\ws\dev\vso\hkcorp\util\typec\src\TypeC\SharedLibs2\"/>
	 <mapping namespace="interface1" from="TypeC.Tests.Shared.Interface1, TypeC.Tests.Shared" to="TypeC.Tests.Lib3.DependentClass, TypeC.Tests.Lib3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
	 <mapping namespace="writer1" from="TypeC.Tests.Shared.IWriter,TypeC.Tests.Shared" to="TypeC.Tests.Lib1.Lib1Writer1, TypeC.Tests.Lib1"/>
	 <mapping namespace="writer2" from="TypeC.Tests.Shared.IWriter,TypeC.Tests.Shared" to="TypeC.Tests.Lib1.Lib1Writer2, TypeC.Tests.Lib1"/>
	 <mapping namespace="gwriter1" from="TypeC.Tests.Shared.IGenericWriter`1[[System.String, mscorlib]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib1.Lib1GenericWriter1, TypeC.Tests.Lib1"/>
	 <mapping namespace="gwriter6" from="TypeC.Tests.Shared.IGenericWriter`1[[TypeC.Tests.Model.MyClass, TypeC.Tests.Model]], TypeC.Tests.Shared" to ="TypeC.Tests.Lib3.Lib3GenericWriter2, TypeC.Tests.Lib3"/>
	</typecmap>

Please note that I haven't tested the security implications of loading from directories that are not in the default probing paths. 