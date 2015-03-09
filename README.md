####Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information.####

#TypeC - A Simple .NET Type Container#
TypeC is a simple .NET type container for resolving interfaces/abstract base classes to their concrete implementations. TypeC also implements factory method for instantiating types. Only one container can exist in a given .NET AppDomain as it is implemented as a singleton. 

The following snippets show sample usage:

    //during the startup 
	TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>();
	
	//deep inside the layers of the code
	var list = typeC.GetInstance<IList<string>>();
	list.Add("test");

With namespace support:
	
	//during the startup 
	string namespace = "MyNamespace"; 
	TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>(namespace);

	//deep inside the layers of the code
	var list = typeC.GetInstance<IList<string>>(namespace);
	list.Add("test"); 

Loading type mapping from a configuration file:

	//during the startup
	string fileName = "TypeConfig.xml";
	TypeContainer tc = TypeContainer.Instance;
	tc.Reset();
	//the Load(fileName) will fail if types can't be resolved 
	tc.Load(fileName);
	

	//deep inside the layers of the code
	IGenericWriter<MyClass> writer1 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter3");
	writer1.Write(new MyClass());
	IGenericWriter<MyClass> writer2 = tc.GetInstance<IGenericWriter<MyClass>>("gwriter4");

Sample XML configuration file is located in TypeC.Tests project. The file follows the standard reflection notation: 
[https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx "Text Notation for Describing .NET Type")

TypeC.GetRegistryAsXml() will dump the registry for easy configuration file creation and debugging.

