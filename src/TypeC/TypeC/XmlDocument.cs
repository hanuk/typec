using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeC
{
    public class XmlDocument
    {
        XDocument _xdoc;
        public XmlDocument(string xml)
        {
            Load(xml);
        }
        void Load(string xml)
        {
            _xdoc = XDocument.Load(new StringReader(xml));

        }

        public List<TypeMapConfigItem> GetTypeMapList()
        {
            List<TypeMapConfigItem> typeMapList = new List<TypeMapConfigItem>();
            foreach (var mapping in _xdoc.Descendants("mapping"))
            {
                var ns = mapping.Attribute("namespace").Value;
                var from = mapping.Attribute("from").Value;
                var to = mapping.Attribute("to").Value;
                typeMapList.Add(new TypeMapConfigItem { Namespace = ns, FromTypeName = from, ToTypeName = to });
            }
            return typeMapList;
        }

        public List<ProbingAssemblyDirectory> GetAssemblyFileList()
        {
            List<ProbingAssemblyDirectory> assemblyPaths = new List<ProbingAssemblyDirectory>();
            foreach (var assemblyPath in _xdoc.Descendants("probinglibpath"))
            {
                ProbingAssemblyDirectory probingAssemblyPath = new ProbingAssemblyDirectory();
                probingAssemblyPath.DirectoryName = assemblyPath.Attribute("path").Value;
                assemblyPaths.Add(probingAssemblyPath);
            }

            return assemblyPaths;
        }
    }
}
