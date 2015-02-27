#TypeC - A Simple .NET Type Container#
TypeC is a simple .NET type container for resolving interfaces/abstract base classes to their concrete implementations. TypeC also implements factory method for instantiating types. Only one container can exist in a given .NET AppDomain as it is implemented as a singleton. 

The following snippets show sample usage:

    //during the startup 
	TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>();
	
	//Deep in the code
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