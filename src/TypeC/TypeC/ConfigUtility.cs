using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeC
{
	public class ConfigUtility
	{
		public static string GetXml(Dictionary<string, Dictionary<Type, Type>> typeRegistry)
		{
			string typeMapTemplate = "<mapping namespace=\"{0}\" from=\"{1}\" to=\"{2}\"/>\n";
			StringBuilder sb = new StringBuilder();
			//sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n");
			sb.Append("<typecmap>\n");

			foreach (var nameSpace in typeRegistry.Keys)
			{
				var typeMapDictionary = typeRegistry[nameSpace];
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

		public static List<TypeMapConfigItem> GetTypeMapFromString(string xml)
		{
			XDocument xdoc = XDocument.Load(new StringReader(xml));
			List<TypeMapConfigItem> typeMapList = new List<TypeMapConfigItem>();
			foreach (var mapping in xdoc.Descendants("mapping"))
			{
				var ns = mapping.Attribute("namespace").Value;
				var from = mapping.Attribute("from").Value;
				var to = mapping.Attribute("to").Value;
				typeMapList.Add(new TypeMapConfigItem { Namespace = ns, FromTypeName = from, ToTypeName = to });
			}
			return typeMapList;
		}

		public static List<TypeMapConfigItem> GetTypeMapFromFile(string xmlFileName)
		{
			string xml = File.ReadAllText(xmlFileName);
			return GetTypeMapFromString(xml);
		}
	}
}
