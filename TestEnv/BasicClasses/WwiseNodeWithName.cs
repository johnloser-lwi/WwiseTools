using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools.Basic
{

    /// <summary>
    /// 包含名称参数的节点
    /// </summary>
    public class WwiseNodeWithName:WwiseNode//, IWwiseName
    {
        /// <summary>
        /// 创建一个包含类型、名称的节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        public WwiseNodeWithName(string u_type, string name, WwiseParser parser) : base (u_type, parser)
        {
            node.SetAttribute("Name", name);
        }


        /// <summary>
        /// 创建一个包含类型、名称的节点，并添加一个子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        /// <param name="child"></param>
        public WwiseNodeWithName(string u_type, string name, WwiseParser parser, WwiseNode child) : base(u_type, parser, child)
        {
            node.SetAttribute("Name", name);
        }

        /// <summary>
        /// 创建一个包含类型、名称的节点，并添加一组子节点
        /// </summary>
        /// <param name="u_type"></param>
        /// <param name="name"></param>
        /// <param name="children"></param>
        public WwiseNodeWithName(string u_type, string name, WwiseParser parser, List<WwiseNode> children) : base(u_type, parser, children)
        {
            node.SetAttribute("Name", name);
        }

        public string Name => node.GetAttribute("Name");
        //string unit_name;
    }
}
