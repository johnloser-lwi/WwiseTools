using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WwiseTools.Basics;

namespace WwiseTools.Utils
{
    /// <summary>
    /// 用于解析现有工作单元(Work Unit)的工具
    /// </summary>
    public class WwiseParser
    {
        //public string[] WorkUnit => workUnit;
        //string[] workUnit;

        public string FilePath => path;
        string path;

        public WwiseParser()
        {
            xmlDocument = new XmlDocument();

            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }
        }

        internal WwiseParser(XmlDocument xmlDocument)
        {
            this.xmlDocument = xmlDocument;

            if (WwiseUtility.ProjectPath == null)
            {
                Console.WriteLine("WwiseUtility not initialized!");
                return;
            }
        }

        public static WwiseParser CreateParserFromNode(WwiseNode node)
        {
            return new WwiseParser(node.XML);
        }

        public XmlDocument Document => xmlDocument; // Temp
        private XmlDocument xmlDocument;

        /// <summary>
        /// 根据文件路径解析工作单元，并返回一个字符串数组
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
        public void Parse(string file_path)
        {
            string _path = Path.Combine(WwiseUtility.ProjectPath, file_path);

            xmlDocument.Load(_path);
        }

        public void ToFile(string path)
        {
            xmlDocument.Save(path);
        }

        /// <summary>
        /// 将对于该工作单元的修改保存，设置是否为原始工作单元创建备份(默认为true)
        /// </summary>
        /// <param name="backup"></param>
        public void CommitChange(bool backup = true)
        {
            File.Copy(path, path + ".backup", true);
            ToFile(path);
        }


        /// <summary>
        /// 为工作单元添加子单元
        /// </summary>
        /// <param name="child"></param>
        public void AddChildToWorkUnit(WwiseUnit child)
        {

            try
            {
                
                XmlNode workUnit = xmlDocument.GetElementsByTagName("WorkUnit")[0];
                XmlNode childlist = workUnit.FirstChild;
                if (childlist == null)
                {
                    childlist = xmlDocument.CreateElement("ChildrenList");
                    workUnit.AppendChild(childlist);
                }

                if (childlist != null)
                {
                    childlist.AppendChild(xmlDocument.ImportNode(child.Node, true));
                    //workUnit.AppendChild(childlist);
                }

                
            }
            catch
            {
                Console.WriteLine("Error adding to Work Unit!");
            }
            

        }

        public void AddChildToUnit(string unitName, string unitType, string id, WwiseUnit child)
        {
            XmlNodeList list = xmlDocument.GetElementsByTagName(unitType);
            foreach (XmlElement el in list)
            {
                if (el.GetAttribute("Name") == unitName && el.GetAttribute("ID").Replace("{", "").Replace("}", "").Trim() == id)
                {
                    el.AppendChild(xmlDocument.ImportNode(child.Node, true));
                }
            }
        }

        /// <summary>
        /// 通过名称搜索单元，需要一个parser的字符串数组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public wwiseObject GetUnitByName(string name, string type)
        {
            XmlNodeList elements = xmlDocument.GetElementsByTagName("ChildrenList");
            Console.WriteLine(elements[0].ChildNodes.Count);
            foreach (XmlElement e in elements[0].ChildNodes)
            {
                Console.WriteLine("{0} : {1}", e.Name, e.GetAttribute("Name"));
                if (e.GetAttribute("Name") == name)
                {
                    wwiseObject wu;
                    wu.Name = name;
                    wu.Type = type;
                    wu.ID = e.GetAttribute("ID").Replace("{", "").Replace("}", "").Trim();
                    return wu;
                }

            }

            wwiseObject unit;
            unit.ID = null;
            unit.Name = null;
            unit.Type = null;

            return unit;
        }
        
        /// <summary>
        /// 获取XML模式版本
        /// </summary>
        /// <returns></returns>
        public int GetSchemaVersion()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ToString());
            return Int32.Parse(doc.ChildNodes[1].Attributes[2].Value);
        }

        /// <summary>
        /// 获取指定字符串中的工作单元信息
        /// </summary>
        /// <param name="txt_file"></param>
        /// <returns></returns>
        public wwiseObject GetWorkUnit()
        {
            //Console.WriteLine(doc.ChildNodes[1].Attributes[0].Value);

            XmlElement workUnit = (XmlElement)xmlDocument.GetElementsByTagName("WorkUnit")[0];
            wwiseObject wu;
            wu.Name = workUnit.GetAttribute("Name");
            wu.ID = workUnit.GetAttribute("ID").Replace("{", "").Replace("}", "").Trim();
            wu.Type = xmlDocument.GetElementsByTagName("WwiseDocument")[0].FirstChild.Name;

            
            return wu;
        }
    }
}
