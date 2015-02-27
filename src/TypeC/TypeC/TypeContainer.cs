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
	/// simple factory class that 
	/// </summary>
	public class TypeContainer
	{
		private static readonly string DEFAULT_NAMESPACE = "DEFAULT";
		private static TypeContainer _instance = new TypeContainer();
		private Dictionary<string, Dictionary<Type, Type>> _typeRegistry;

		public static TypeContainer Instance
		{
			get { return _instance; }
		}

		private TypeContainer()
		{
			_typeRegistry = new Dictionary<string, Dictionary<Type, Type>>();
		}
		public void Register<TFromType, TToType>()
		{
			string nameSpace = DEFAULT_NAMESPACE;
			Register<TFromType, TToType>(nameSpace);

		}
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

		public TFromType GetInstance<TFromType>() where TFromType : class
		{
			string nameSpace = DEFAULT_NAMESPACE;
			return GetInstance<TFromType>(nameSpace);
		}
	}
}
