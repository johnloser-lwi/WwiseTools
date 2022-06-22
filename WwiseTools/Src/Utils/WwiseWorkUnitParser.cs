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

        private bool _parsedSuccessfully = false;

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
            try
            {
                _parsedSuccessfully = false;

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"{filePath} doesn't exist!");

                _filePath = filePath;
                XML.Load(filePath);

                _parsedSuccessfully = true;
            }
            catch (Exception e)
            {
                WaapiLog.InternalLog($"Failed to parse file {filePath}! ======> {e.Message}");
            }
        }

        /// <summary>
        /// 增加xml node至指定对象
        /// </summary>
        /// <param name="wwiseObject"></param>
        /// <param name="node"></param>
        public void AddToUnit(WwiseObject wwiseObject, XmlNode node)
        {
            XmlCheck();

            if (wwiseObject == null) return;
            var target = (XmlElement)GetNodeByID(wwiseObject.ID);
            target?.AppendChild(XML.ImportNode(node, true));
        }

        public XmlNode GetNodeByID(string wwiseId)
        {
            XmlCheck();

            return XML.SelectSingleNode($"//*[@ID='{wwiseId}']");
        }

        public XmlNodeList GetChildrenNodeList(XmlNode node)
        {
            XmlCheck();

            return node.SelectNodes("ChildrenList/*");
        }

        private void XmlCheck()
        {
            if (!_parsedSuccessfully)
                throw new Exception($"{nameof(WwiseWorkUnitParser)} doesn't have a valid XML file parsed!");
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
