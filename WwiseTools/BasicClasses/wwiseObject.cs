using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WwiseTools.Utils;
using WwiseTools.Basics;
using System.Xml;

namespace WwiseTools
{

    public struct wwiseObject
    {
        public string Name;
        public string Type;
        public string ID;
    }

    /*
    /// <summary>
    /// Wwise的工作单元
    /// </summary>
    /// 
    public class WwiseWorkUnit : WwiseUnit//, IWwiseID, IWwisePrintable
    {

        //public override int tabs { get => 0; set { return; } }

        public override string Type => type;
        private string type;
        private XmlElement workUnit;

        /// <summary>
        /// 获取工作单元的xml模式版本
        /// </summary>
        public int SchemaVersion { get => WwiseUtility.SchemaVersion; }

        /// <summary>
        /// 创建一个包含名称、类型的工作单元
        /// </summary>
        /// <param name="name"></param>
        /// <param name="u_type"></param>
        public WwiseWorkUnit(string name, string workUnitType, WwiseParser parser) : base(name, "WwiseDocument", parser)
        {
            Init(name, workUnitType, Guid.NewGuid().ToString().ToUpper());
            childrenList = new WwiseNode("ChildrenList", parser);
            AddChildrenList();
        }

        /// <summary>
        /// 创建一个包含名称、类型的工作单元，并设置GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="u_type"></param>
        /// <param name="guid"></param>
        public WwiseWorkUnit(string name, string workUnitType, string guid, WwiseParser parser) : base(name, "WwiseDocument", guid, parser)
        {

            Init(name, workUnitType, guid);
            childrenList = new WwiseNode("ChildrenList", parser);
            AddChildrenList();
        }


        private void Init(string name, string workUnitType, string guid)
        {
            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><WwiseDocument></WwiseDocument>");
            XmlElement document = (XmlElement)xmlDocument.GetElementsByTagName("WwiseDocument")[0];
            document.SetAttribute("Type", "WorkUnit");
            document.SetAttribute("ID", "{" + guid + "}");
            document.SetAttribute("SchemaVersion", WwiseUtility.SchemaVersion.ToString());
            XmlElement type = xmlDocument.CreateElement(workUnitType);
            XmlElement workUnit = xmlDocument.CreateElement("WorkUnit");
            workUnit.SetAttribute("Name", name);
            workUnit.SetAttribute("ID", "{" + guid + "}");
            workUnit.SetAttribute("PersistMode", "Standalone");

            type.AppendChild(workUnit);
            document.AppendChild(type);
            xmlDocument.AppendChild(document);

            this.node = document;
            this.type = type.Name;

            foreach (XmlElement c in ChildNodes)
            {
                if (c.Name == "ChildrenList") return;
            }

            workUnit.AppendChild(childrenList.Node);
        }

        protected override void AddChildrenList()
        {
        }
    }*/
}
