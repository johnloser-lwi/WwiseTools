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

        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="file_path"></param>
        public void Parse(string file_path)
        {
            filePath = file_path;
            XML.Load(file_path);
        }

        /// <summary>
        /// 增加xml node至指定对象
        /// </summary>
        /// <param name="object"></param>
        /// <param name="node"></param>
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

        /// <summary>
        /// 保存文件
        /// </summary>
        public void SaveFile()
        {
            XML.Save(filePath);
        }
    }
}
