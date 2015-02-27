#TypeC - A Simple .NET Type Container#
TypeC is a simple .NET type container for resolving interfaces/abstract base classes to their concrete implementations. TypeC also implements factory method for instantiating types. Only one container can exist in a given .NET AppDomain as it is implemented as a singleton. 

The following snippets show sample usage:

    TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>();
	var list = typeC.GetInstance<IList<string>>();
	list.Add("test");

With namespace support:
	
	string namespace = "MyNamespace"; 
	TypeContainer typeC = TypeContainer.Instance;
	typeC.Register<IList<string>, List<string>>(namespace);
	var list = typeC.GetInstance<IList<string>>(namespace);
	list.Add("test"); 