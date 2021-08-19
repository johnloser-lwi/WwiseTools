using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Objects;

namespace WwiseTools.Utils
{
    public class WwiseWorkUnitParser
    {
        public XmlDocument XML { get; private set; }
        private string filePath;

        public WwiseWorkUnitParser(string file_path)
        {
            XML = new XmlDocument();
            Parse(file_path);
        }

        public void Parse(string file_path)
        {
            filePath = file_path;
            XML.Load(file_path);
        }

        public void AddToUnit(WwiseObject @object, XmlNode node)
        {
            XmlNodeList list = XML.GetElementsByTagName(@object.Type);
            foreach (XmlElement el in list)
            {
                if (el.GetAttribute("ID") == @object.ID)
                {
                    el.AppendChild(XML.ImportNode(node, true));
                }
            }
        }

        public void SaveFile()
        {
            XML.Save(filePath);
        }
    }
}
