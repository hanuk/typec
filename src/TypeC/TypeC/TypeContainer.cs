/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information 
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace TypeC
{
	/// <summary>
	/// simple factory class that manage type registration and dependency resolution. This doesn't do any dependency injection. 
	/// </summary>
	public class TypeContainer
	{
		/// <summary>
		/// Type registration without namespace goes into DEFAULT namespace
		/// </summary>
		private static readonly string DEFAULT_NAMESPACE = "DEFAULT";
		/// <summary>
		/// Singleton instance
		/// </summary>
		private static TypeContainer _instance = new TypeContainer();
		/// <summary>
		/// Registered types are stored in nested dictionaries 
		/// </summary>
		private Dictionary<string, Dictionary<Type, Type>> _typeRegistry;

		/// <summary>
		/// static reference that is part of the singleton implementation
		/// </summary>
		public static TypeContainer Instance
		{
			get { return _instance; }
		}

		/// <summary>
		/// private constructor for singleton implementation
		/// </summary>
		private TypeContainer()
		{
			Reset();
		}

		/// <summary>
		/// clears the container state for the purpose of testing; Add #IFDEBUG
		/// </summary>
		public void Reset()
		{
			_typeRegistry = new Dictionary<string, Dictionary<Type, Type>>();
		}
		/// <summary>
		/// Registers .NET Type objects into DEFAULT namespace
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		public void Register<TFromType, TToType>()
		{
			string nameSpace = DEFAULT_NAMESPACE;
			Register<TFromType, TToType>(nameSpace);
		}
		/// <summary>
		/// Registers .NET Type objects into the given namespace
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		/// <param name="nameSpace"></param>
		public void Register<TFromType, TToType>(string nameSpace)
		{
			Type from = typeof(TFromType);
			Type to = typeof(TToType);

			Register(nameSpace, from, to);
		}
		/// <summary>
		/// Creates instance of a .NET Type registered in a given namespace
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		/// <returns>Null the object or Null if not found</returns>
		public TFromType GetInstance<TFromType>(string nameSpace) where TFromType : class
		{
			if (_typeRegistry.ContainsKey(nameSpace))
			{
				return GetInstance<TFromType>(_typeRegistry[nameSpace]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Utility method for Type lookup and instantiation from a given type registry
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		/// <returns>Null the object or null if not found</returns>
		private TFromType GetInstance<TFromType>(Dictionary<Type, Type> typeRegistry) where TFromType : class
		{
			Type from = typeof(TFromType);
			if (typeRegistry.ContainsKey(from))
			{
				Type toType = typeRegistry[from];
				return Activator.CreateInstance(toType) as TFromType;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// Creates instance of a .NET Type registered in a DEFAULT namespace
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		/// <returns>Null the object or null if not found</returns>
		public TFromType GetInstance<TFromType>() where TFromType : class
		{
			string nameSpace = DEFAULT_NAMESPACE;
			return GetInstance<TFromType>(nameSpace);
		}
		/// <summary>
		/// Appends type mapping information into the current instance. use Reset()
		/// before calling Load(fileName) for removing the previous type mapping information 
		/// </summary>
		/// <param name="fileName"></param>
		public void Load(string fileName)
		{
			var typeMapList = GetTypeMap(fileName);
			Load(typeMapList);
		}
		/// <summary>
		/// Reads the file and returns the TypeMapItem list
		/// </summary>
		/// <param name="fileName">XML file containing type information</param>
		/// <returns></returns>
		private List<TypeMapItem> GetTypeMap(string fileName)
		{
			XDocument xdoc = XDocument.Load(fileName);
			List<TypeMapItem> typeMapList = new List<TypeMapItem>();
			foreach (var mapping in xdoc.Descendants("mapping"))
			{
				var ns = mapping.Attribute("namespace").Value;
				var from = mapping.Attribute("from").Value;
				var to = mapping.Attribute("to").Value;
				typeMapList.Add(new TypeMapItem { Namespace = ns, FromTypeName = from, ToTypeName = to });
			}
			return typeMapList;
		}
		/// <summary>
		/// Appends type mapping information into the current instance. use Reset()
		/// before calling Load(typeMapItems) for removing the previous type mapping information 
		/// </summary>
		/// <param name="typeMapItems"></param>
		public void Load(List<TypeMapItem> typeMapItems)
		{
			foreach (var typeMapItem in typeMapItems)
			{
				Load(typeMapItem);
			}
		}
		/// <summary>
		/// Appends a single type mapping  into the current instance. use Reset()
		/// before calling Load(typeMapItem) for removing the previous type mapping information 
		/// </summary>
		/// <param name="typeMapItem"></param>
		public void Load(TypeMapItem typeMapItem)
		{
			Type from = Type.GetType(typeMapItem.FromTypeName);
			Type to = Type.GetType(typeMapItem.ToTypeName);

			//check if the types are resolved
			if (from == null || to == null)
			{
				throw new ApplicationException("From or To  not found");
			}
			
			//validate for default constructor and subtyping
			if (!IsMappingValid(from, to))
			{
				throw new ApplicationException(string.Format("In {0} : can't assign {1} to {2}", MethodBase.GetCurrentMethod().Name, to.Name, from.Name ));
			}

			//if namespace is not found register into default namespace
			string nameSpace = typeMapItem.Namespace;
			if (nameSpace.Length == 0)
			{
				Register(from, to);
			}
			else
			{
				Register(nameSpace, from, to);
			}
		}

		private static bool IsMappingValid(Type from, Type to)
		{
			//"from" has to be a class or an interface
			if (!(from.IsClass || from.IsInterface))
			{
				return false;
			}

			if (!to.IsClass || to.GetConstructor(Type.EmptyTypes) == null)
			{
				return false; 
			}

			//"from" has to be a subtype of "to"
			if(!from.IsAssignableFrom(to))
			{
				return false; 
			}

			return true; 
		}
		/// <summary>
		/// Registers a type map into default namespace
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		private void Register(Type from, Type to)
		{
			string nameSpace = DEFAULT_NAMESPACE;
			Register(nameSpace, from, to);
		}

		/// <summary>
		/// Registers a type map into a given namespace
		/// </summary>
		/// <param name="nameSpace"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		private void Register(string nameSpace, Type from, Type to)
		{
			if (!_typeRegistry.ContainsKey(nameSpace))
			{
				_typeRegistry.Add(nameSpace, new Dictionary<Type, Type>());
			}
			var registry = _typeRegistry[nameSpace];

			if (registry.ContainsKey(from))
			{
				registry[from] = to;
			}
			else
			{
				registry.Add(from, to);
			}
		}
		
	}
}
