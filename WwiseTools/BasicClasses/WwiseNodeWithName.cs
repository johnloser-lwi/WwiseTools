using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Basic
{

    /// <summary>
    /// 包含名称参数的节点
    /// </summary>
    public class WwiseNodeWithName :WwiseNode, IWwiseName
    {
        /// <summary>
        /// 创建一个包含类型、名称的节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        public WwiseNodeWithName(string u_type, string name) : base (u_type)
        {
            unit_name = name;
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();
        }


        /// <summary>
        /// 创建一个包含类型、名称的节点，并添加一个子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        /// <param name="child"></param>
        public WwiseNodeWithName(string u_type, string name, IWwisePrintable child) : base(u_type)
        {
            unit_name = name;
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            AddChildNode(child);
        }

        /// <summary>
        /// 创建一个包含类型、名称的节点，并添加一组子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        /// <param name="children"></param>
        public WwiseNodeWithName(string u_type, string name, List<IWwisePrintable> children) : base(u_type)
        {
            unit_name = name;
            this.u_type = u_type;
            xml_head = String.Format("<{0} Name=\"{1}\">", u_type, name);
            xml_tail = String.Format("</{0}>", u_type);
            xml_body = new List<IWwisePrintable>();

            foreach (var c in children)
            {
                AddChildNode(c);
            }
        }

        public string name => unit_name;
        string unit_name;
    }
}
