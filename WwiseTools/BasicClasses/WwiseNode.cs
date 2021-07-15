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
    public class WwiseNode
    {
        /// <summary>
        /// 创建一个子单元列表(Children List)
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static WwiseNode NewChildrenList(WwiseParser parser)
        {
            
            return new WwiseNode("ChildrenList", parser);
        }

        /// <summary>
        /// 创建一个参数列表(Property List)
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static WwiseNode NewPropertyList(WwiseParser parser)
        {
            if (!CheckParser(parser))
            {
                Console.WriteLine("Parser is null!");
                return null;
            }
            return new WwiseNode("PropertyList", parser);
        }

        /// <summary>
        /// 创建一个引用列表(Reference List)
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static WwiseNode NewReferenceList(WwiseParser parser)
        {
            if (!CheckParser(parser))
            {
                Console.WriteLine("Parser hasn't parsed any Work Unit!");
                return null;
            }
            return new WwiseNode("ReferenceList", parser);
        }

        protected static bool CheckParser(WwiseParser parser)
        {
            if (parser == null)
            {
                Console.WriteLine("Parser is null!");
                return false;
            }
            if (parser.Document == null)
            {
                Console.WriteLine("Parser has not parsed any Work Unit!");
                return false;
            }
            return true;
        }

        public virtual string Type => node.Name;

        public virtual XmlDocument XML => xmlDocument;

        protected WwiseParser parser;

        public  XmlNodeList ChildNodes => node.ChildNodes;

        protected XmlDocument xmlDocument;
        protected XmlElement node;

        public XmlElement Node => node;

        public XmlAttributeCollection Attributes => node.Attributes;

        /// <summary>
        /// 创建一个只包含类型参数的节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="parser"></param>
        public WwiseNode(string u_type, WwiseParser parser)
        {
            if (!CheckParser(parser))
            {
                Console.WriteLine("Parser is null!");
                return;
            }
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一个子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="parser"></param>
        /// <param name="child"></param>
        public WwiseNode(string u_type, WwiseParser parser, WwiseNode child)
        {
            if (!CheckParser(parser))
            {
                Console.WriteLine("Parser is null!");
                return;
            }
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);

            this.AddChildNode(child);
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一组子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="parser"></param>
        /// <param name="children"></param>
        public WwiseNode(string u_type, WwiseParser parser, List<WwiseNode> children)
        {
            if (!CheckParser(parser))
            {
                Console.WriteLine("Parser is null!");
                return;
            }
            this.parser = parser;
            xmlDocument = parser.Document;
            node = xmlDocument.CreateElement(u_type);

            foreach (var c in children)
            {
                AddChildNode(c);
            }
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveChildNode(WwiseNode node)
        {
            if (node == null)
            {
                Console.WriteLine("Node is null!");
                return;
            }

            Node.RemoveChild(node.Node);
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChildNode(XmlElement node)
        {
            if (node == null)
            {
                Console.WriteLine("Node is null!");
                return;
            }
            Node.RemoveChild(node);
        }

        /// <summary>
        /// 添加子节点，并返回该节点，如果子节点无效则返回null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public WwiseNode AddChildNode(WwiseNode node)
        {
            if (node == null)
            {
                Console.WriteLine("Node is null!");
                return null;
            }
            XmlElement n = node.Node;
            this.node.AppendChild(n);
            return node;
        }

        /// <summary>
        /// 在节点开头添加子节点
        /// </summary>
        /// <param name="node"></param>
        public void AddChildNodeAtFront(WwiseNode node)
        {
            if (node == null)
            {
                Console.WriteLine("Node is null!");
                return;
            }
            this.node.PrependChild(node.Node);
        }

        /// <summary>
        /// 将节点信息转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
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
