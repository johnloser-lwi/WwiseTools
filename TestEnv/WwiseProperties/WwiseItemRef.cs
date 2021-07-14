using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Utils;

namespace WwiseTools.Properties
{
    /// <summary>
    /// 对于子单元的引用
    /// </summary>
    public class WwiseItemRef : WwiseNodeWithName
    {
        /// <summary>
        /// 初始化子单元的名称以及GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseItemRef(string name, string id, WwiseParser parser) : base("ItemRef", name, parser)
        {
            node.SetAttribute("ID", "{" + id + "}");
        }
    }
}
