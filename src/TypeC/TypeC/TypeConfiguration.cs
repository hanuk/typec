using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeC
{
    /// <summary>
    /// TypeConfiguration represetnts the state of the container represented by list of probing paths and type map list
    /// No additional responsibilities to be added for this class
    /// </summary>
    class TypeConfiguration
    {
        private List<ProbingAssemblyDirectory> _probingAssemblyPathList;
        private List<TypeMapConfigItem> _typeMapList;
        public TypeConfiguration()
        {
            _probingAssemblyPathList = new List<ProbingAssemblyDirectory>();
            _typeMapList = new List<TypeMapConfigItem>();
        }
        /// <summary>
        /// erases the previous type configuration
        /// </summary>
        /// <param name="xmlFileName">name of the xml configuration file</param>
        /// <returns>TypeConfiguration</returns>

        public void LoadFromFile(string xmlFileName)
        {
            string xml = File.ReadAllText(xmlFileName);
            Load(xml);
        }

        public void Load(string xml)
        {
            XmlDocument _xmlConfig = new XmlDocument(xml);
            this._probingAssemblyPathList.AddRange(_xmlConfig.GetAssemblyFileList());
            this._typeMapList.AddRange(_xmlConfig.GetTypeMapList());
        }
        public List<TypeMapConfigItem> TypeMapList
        {
            get { return _typeMapList; }
        }
        public List<ProbingAssemblyDirectory> ProbingAssemblyPathList
        {
            get { return _probingAssemblyPathList; }
        }
        /// <summary>
        /// this can be used to generate XML configuraiton file
        /// </summary>
        /// <returns></returns>
        public string GetProbingDirectoryListAsXml()
        {
            string xmlTemplate = @"<probinglibpath path=""{0}""/>\n";
            StringBuilder sb = new StringBuilder();
            foreach (var kap in _probingAssemblyPathList)
            {
                sb.Append(string.Format(xmlTemplate, kap.DirectoryName));
            }
            return sb.ToString();
        }


        public string GetAssemblyPath(string assemblyName)
        {

            foreach (var pap in _probingAssemblyPathList)
            {
                foreach (var dllName in pap.DllNames)
                {
                    if (dllName.Equals(assemblyName + ".dll"))
                    {
                        return pap.DirectoryName + @"\" + dllName;
                    }
                }
            }
            return null; 
        }
    }
}
