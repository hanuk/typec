/*  
 Copyright (c) Microsoft.  All rights reserved.  
  
Licensed under the Apache License, Version 2.0 (the "License");  
you may not use this file except in compliance with the License.  
You may obtain a copy of the License at //   http://www.apache.org/licenses/LICENSE-2.0  
  
Unless required by applicable law or agreed to in writing, software  
distributed under the License is distributed on an "AS IS" BASIS,  
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if (!_typeRegistry.ContainsKey(nameSpace))
			{
				_typeRegistry.Add(nameSpace, new Dictionary<Type, Type>());
			}
			var registry = _typeRegistry[nameSpace];
			Type from = typeof(TFromType);
			Type to = typeof(TToType);
			if (registry.ContainsKey(from))
			{
				registry[from] = to;
			}
			else
			{
				registry.Add(from, to);
			}
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
	}
}
