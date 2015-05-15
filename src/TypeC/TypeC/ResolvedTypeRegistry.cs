using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeC
{
    internal class ResolvedTypeRegistry
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Type, Type>> _typeRegistry;
        private ConcurrentBag<string> _errors; 
        public ResolvedTypeRegistry()
        {
            _errors = new ConcurrentBag<string>();
            _typeRegistry = new ConcurrentDictionary<string, ConcurrentDictionary<Type, Type>>();
        }
        internal ConcurrentDictionary<string, ConcurrentDictionary<Type, Type>> TypeRegistry 
        {
            get { return _typeRegistry; }
        }

        /// <summary>
        /// Registers a type map into a given namespace
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        internal void Register(string nameSpace, Type from, Type to)
        {
            //if the namespace doesn't exist, create one
            if (!_typeRegistry.ContainsKey(nameSpace))
            {
                _typeRegistry[nameSpace] = new ConcurrentDictionary<Type, Type>();
            }

            var registry = _typeRegistry[nameSpace];


            if (registry.ContainsKey(from))
            {
                registry[from] = to;
            }
            else
            {
                registry.TryAdd(from, to);
            }
        }


        public ConcurrentBag<string> Errors
        {
            get { return _errors; }
        }
        internal string GetResolvedTypeRegistryAsXml()
        {
            string typeMapTemplate = "<mapping namespace=\"{0}\" from=\"{1}\" to=\"{2}\"/>\n";
            StringBuilder sb = new StringBuilder();
            sb.Append("<typecmap>\n");

            foreach (var nameSpace in _typeRegistry.Keys)
            {
                var typeMapDictionary = _typeRegistry[nameSpace];
                foreach (var fromType in typeMapDictionary.Keys)
                {
                    string fromTypeName = fromType.FullName + ", " + fromType.Assembly.FullName;
                    Type toType = typeMapDictionary[fromType];
                    string toTypeName = toType.FullName + ", " + toType.Assembly.FullName;

                    sb.Append(string.Format(typeMapTemplate, nameSpace, fromTypeName, toTypeName));
                }
            }
            sb.Append("</typecmap>\n");

            return sb.ToString();
        }
    }
}
