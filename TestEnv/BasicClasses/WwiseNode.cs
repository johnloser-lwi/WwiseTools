using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using WwiseTools.Utils;

namespace WwiseTools.Basics
{
    /// <summary>
    /// 一切可以添加至wwu文件中的内容均为节点(Node)
    /// </summary>
    public class WwiseNode //: IWwiseNode, IWwisePrintable
    {
        /// <summary>
        /// 创建一个子单元列表(Children List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewChildrenList(WwiseParser parser)
        {
            return new WwiseNode("ChildrenList", parser);
        }

        /// <summary>
        /// 创建一个参数列表(Property List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewPropertyList(WwiseParser parser)
        {
            return new WwiseNode("PropertyList", parser);
        }

        /// <summary>
        /// 创建一个引用列表(Reference List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewReferenceList(WwiseParser parser)
        {
            return new WwiseNode("ReferenceList", parser);
        }

        //public virtual int tabs { get => p_tabs; set { p_tabs = value; } }

        public virtual string Type => node.Name;

        public virtual XmlDocument XML => xmlDocument;

        protected WwiseParser parser;

        //public string head => xml_head;

        //public string tail => xml_tail;

        public  XmlNodeList ChildNodes => node.ChildNodes;

        // protected int p_tabs = 0;

        //protected string u_type;
        //protected string xml_head;
        //protected string xml_tail;
        //protected List<IWwisePrintable> xml_body;

        protected XmlDocument xmlDocument;
        protected XmlElement node;

        public XmlElement Node => node;

        public XmlAttributeCollection Attributes => node.Attributes;

        /// <summary>
        /// 创建一个只包含类型参数的节点
        /// </summary>
        /// <param name="u_type"></param>
        public WwiseNode(string u_type, WwiseParser parser)
        {
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一个子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="child"></param>
        public WwiseNode(string u_type, WwiseParser parser, WwiseNode child)
        {
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);

            this.AddChildNode(child);
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一组子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="children"></param>
        public WwiseNode(string u_type, WwiseParser parser, List<WwiseNode> children)
        {
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);

            foreach (var c in children)
            {
                AddChildNode(c);
            }
        }

        /// <summary>
        /// 根据索引删除子节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveChildNode(WwiseNode node)
        {
            Node.RemoveChild(node.Node);
        }

        public void RemoveChildNode(XmlElement node)
        {
            Node.RemoveChild(node);
        }


        public WwiseNode AddChildNode(WwiseNode node)
        {
            XmlElement n = node.Node;
            this.node.AppendChild(n);
            return node;
        }

        public void AddChildNodeAtFront(WwiseNode node)
        {
            this.node.PrependChild(node.Node);
        }

        public virtual string Print()
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                Node.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public virtual wwiseObject ToObject()
        {
            wwiseObject result;
            result.Type = Type;
            result.Name = null;
            result.ID = null;
            return result;
        }

    }
}
