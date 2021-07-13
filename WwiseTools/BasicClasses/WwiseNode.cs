using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Basic
{
    /// <summary>
    /// 一切可以添加至wwu文件中的内容均为节点(Node)
    /// </summary>
    public class WwiseNode : IWwiseNode, IWwisePrintable
    {
        /// <summary>
        /// 创建一个子单元列表(Children List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewChildrenList(List<IWwisePrintable> children)
        {
            return new WwiseNode("ChildrenList", children);
        }

        /// <summary>
        /// 创建一个参数列表(Property List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewPropertyList(List<IWwisePrintable> children)
        {
            return new WwiseNode("PropertyList", children);
        }

        /// <summary>
        /// 创建一个引用列表(Reference List)
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static WwiseNode NewReferenceList(List<IWwisePrintable> children)
        {
            return new WwiseNode("ReferenceList", children);
        }

        public virtual int tabs { get => p_tabs; set { p_tabs = value; } }

        public string type => u_type;

        public string head => xml_head;

        public string tail => xml_tail;

        public List<IWwisePrintable> body => xml_body;

        protected int p_tabs = 0;

        protected string u_type;
        protected string xml_head;
        protected string xml_tail;
        protected List<IWwisePrintable> xml_body;

        /// <summary>
        /// 创建一个只包含类型参数的节点
        /// </summary>
        /// <param name="u_type"></param>
        public WwiseNode(string u_type)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0}>", u_type);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一个子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="child"></param>
        public WwiseNode(string u_type, IWwisePrintable child)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0}>", u_type);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            this.AddChildNode(child);
        }

        /// <summary>
        /// 创建一个只包含类型参数的节点，并添加一组子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="children"></param>
        public WwiseNode(string u_type, List<IWwisePrintable> children)
        {
            this.u_type = u_type;
            xml_head = String.Format("<{0}>", u_type);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            foreach (var c in children)
            {
                AddChildNode(c);
            }
        }

        /// <summary>
        /// 根据索引删除子节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveChildNode(int index)
        {
            xml_body.RemoveAt(index);
        }

        
        public IWwisePrintable AddChildNode(IWwisePrintable node)
        {
            body.Add(node);
            return node;
        }

        public void AddChildNodeAtFront(IWwisePrintable node)
        {
            body.Insert(0, node);
        }

        /// <summary>
        /// 将内容输出为字符串，并设置是否从新的一行开始输出
        /// </summary>
        /// <param name="startAtNewLine"></param>
        /// <returns>string</returns>
        public virtual string Print(bool startAtNewLine)
        {
            string t = "";
            for (int i = 0; i < tabs; i++)
            {
                t += "\t";
            }
            string result = "";
            if (startAtNewLine) result += "\n";
            result += t + head;
            foreach (IWwisePrintable u in body)
            {
                if (u == null) continue;
                u.tabs = tabs + 1;
                result += u.Print();
            }

            result += "\n" + t + tail;

            return result;
        }

        public virtual string Print()
        {
            string t = "";
            for (int i = 0; i < tabs; i++)
            {
                t += "\t";
            }
            string result = "";
            result += "\n";
            result += t + head;
            foreach (IWwisePrintable u in body)
            {
                if (u == null) continue;
                u.tabs = tabs + 1;
                result += u.Print();
            }

            result += "\n" + t + tail;

            return result;
        }
    }
}
