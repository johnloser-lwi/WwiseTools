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
        private string _filePath;

        public WwiseWorkUnitParser(string filePath)
        {
            XML = new XmlDocument();
            Parse(filePath);
        }

        /// <summary>
        /// 解析文件
        /// </summary>
        /// <param name="filePath"></param>
        public void Parse(string filePath)
        {
            _filePath = filePath;
            XML.Load(filePath);
        }

        /// <summary>
        /// 增加xml node至指定对象
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="node"></param>
        public void AddToUnit(WwiseObject wwiseObject, XmlNode node)
        {
            if (wwiseObject == null) return;
            var target = (XmlElement)GetNodeByID(wwiseObject.ID);
            target?.AppendChild(XML.ImportNode(node, true));
        }

        public XmlNode GetNodeByID(string wwiseId)
        {
            if (XML == null) return null;

            return XML.SelectSingleNode($"//*[@ID='{wwiseId}']");
        }

        public XmlNodeList GetChildrenNodeList(XmlNode node)
        {
            if (XML == null) return null;

            return node.SelectNodes("ChildrenList/*");
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void SaveFile()
        {
            XML.Save(_filePath);
        }
    }
}
