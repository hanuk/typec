/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See License.txt in the project root for license information 
*/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
		/// Registered types are stored in nested dictionaries after validating if these can be resolved
		/// </summary>
        private ResolvedTypeRegistry _resolvedTypeRegistry = new ResolvedTypeRegistry();
        private TypeConfiguration _typeConfig = new TypeConfiguration();
        private Hashtable _assemblyCache; 
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
		public  TypeContainer()
		{
			Reset();
            _assemblyCache = new Hashtable();
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveHandler;
		}
        /// <summary>
        /// Resolves assemblies from the custom probing paths managed by TypeConfiguration
        /// </summary>
        /// <param name="sender">Program that is trying to resolve the assembly</param>
        /// <param name="args">Containes name of the assembly to be resolved and the name of the referencing assembly</param>
        /// <returns></returns>
        Assembly AssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            //convert it to short name
            string shortName = args.Name;
            if (shortName.IndexOf("Version=") != -1)
            {
                shortName = shortName.Substring(0, shortName.IndexOf(","));
            }
            if (_assemblyCache.Contains(shortName))
            {
                return (Assembly)_assemblyCache[shortName];
            }
            string dllPathName = _typeConfig.GetAssemblyPath(shortName);
            if (dllPathName == null)
            {
                return null; 
            }
            Assembly asm = Assembly.LoadFile(dllPathName);
            _assemblyCache.Add(shortName, asm);
            return asm;  
        }

		/// <summary>
		/// clears the container state for the purpose of testing; Add #IFDEBUG
		/// </summary>
		public void Reset()
		{
            _resolvedTypeRegistry = new ResolvedTypeRegistry();
            _typeConfig = new TypeConfiguration();
		}
        /// <summary>
        /// add the directory name to the list of directories from which the assemblies are resolved
        /// </summary>
        /// <param name="assemblyPath"></param>
        public void AddAssemblyProbingPath(string assemblyPath)
        {
            _typeConfig.ProbingAssemblyPathList.Add(new ProbingAssemblyDirectory { DirectoryName = assemblyPath });
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

            _resolvedTypeRegistry.Register(nameSpace, from, to);
		}
		/// <summary>
		/// Creates instance of a .NET Type registered in a given namespace
		/// </summary>
		/// <typeparam name="TFromType">The abstract Type</typeparam>
		/// <typeparam name="TToType">The concrete Type</typeparam>
		/// <returns>Null the object or Null if not found</returns>
		public TFromType GetInstance<TFromType>(string nameSpace) where TFromType : class
		{
			if (_resolvedTypeRegistry.TypeRegistry.ContainsKey(nameSpace))
			{
                return GetInstance<TFromType>(_resolvedTypeRegistry.TypeRegistry[nameSpace]);
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
		private TFromType GetInstance<TFromType>(ConcurrentDictionary<Type, Type> typeRegistry) where TFromType : class
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

		public string GetRegistryAsXml()
		{
            //return ConfigUtility.GetXml(_resolvedTypeRegistry.TypeRegistry);
            StringBuilder sb = new StringBuilder();
            sb.Append(_typeConfig.GetProbingDirectoryListAsXml());
            sb.Append(_resolvedTypeRegistry.GetResolvedTypeRegistryAsXml());
            return sb.ToString();
		}
		/// <summary>
		/// Appends type mapping information into the current instance. use Reset()
		/// before calling Load(fileName) for removing the previous type mapping information 
		/// </summary>
		/// <param name="fileName"></param>
        /// <param name="replace">the default is "false" which appends to the existing type map; "true" will replace the contents</param>
		public void LoadFromFile(string fileName, bool replace = false)
		{
            _typeConfig.LoadFromFile(fileName);
            UpdateRegistry(_typeConfig.TypeMapList);

		}

		/// <summary>
		/// xml as a string
		/// </summary>
		/// <param name="xmlConfig"></param>
		public void LoadFromString(string xmlConfig)
		{
            //var typeMapList = ConfigUtility.GetTypeMapFromString(xmlConfig);
            _typeConfig.Load(xmlConfig);
            UpdateRegistry(_typeConfig.TypeMapList);
		}

		/// <summary>
		/// Appends type mapping information into the current instance. use Reset()
		/// before calling Load(typeMapItems) for removing the previous type mapping information 
		/// </summary>
		/// <param name="typeMapItems"></param>
		public void UpdateRegistry(List<TypeMapConfigItem> typeMapItems)
		{
			foreach (var typeMapItem in typeMapItems)
			{
				ResolveType(typeMapItem);
			}

            if (this.Errors.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach(string error in this.Errors)
                {
                    sb.AppendLine(error);
                }
                throw new TypeLoadException(string.Format("Can't resolve some types; error list: {0}", sb.ToString()));
            }
		}

		/// <summary>
		/// checks if the "from" is a subtype of "to"
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static bool IsMappingValid(Type from, Type to)
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
			if (!from.IsAssignableFrom(to))
			{
				return false;
			}

			return true;
		}
        public string[] Errors
        {
            get
            {
                return _resolvedTypeRegistry.Errors.ToArray();   
            }
        }
		/// <summary>
		/// Appends a single type mapping  into the current instance. use Reset()
		/// before calling Load(typeMapItem) for removing the previous type mapping information 
		/// </summary>
		/// <param name="typeMapItem"></param>
		private void ResolveType(TypeMapConfigItem typeMapItem)
		{
            string exceptionMessage = string.Empty;
			//check if "from" is resolved
			Type from = Type.GetType(typeMapItem.FromTypeName);

			//check if "to" is resolved
			Type to = Type.GetType(typeMapItem.ToTypeName);


            if (from == null)
            {
                exceptionMessage = string.Format("{0}: could not be resolved", typeMapItem.FromTypeName);
                _resolvedTypeRegistry.Errors.Add(exceptionMessage);
            }

			if (to == null)
			{
                exceptionMessage = string.Format("{0}: could not be resolved", typeMapItem.ToTypeName);
                _resolvedTypeRegistry.Errors.Add(exceptionMessage);
			}
            if (from == null || to == null)
            {
                //no point in processing further
                return; 
            }
			//validate for default constructor and subtyping
			if (!IsMappingValid(from, to))
			{
                exceptionMessage = string.Format("Can't cast {1} to {2}", to.Name, from.Name);
                _resolvedTypeRegistry.Errors.Add(exceptionMessage);
			}

            if (_resolvedTypeRegistry.Errors.Count != 0)
            {
                return; 
            }
			//if namespace is not found register into default namespace
            if (typeMapItem.Namespace.Length == 0)
			{
                _resolvedTypeRegistry.Register(DEFAULT_NAMESPACE,from, to);
			}
			else
			{
                _resolvedTypeRegistry.Register(typeMapItem.Namespace, from, to);
			}
		}
	}
}
