using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Basics;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// 对于Music Segment的引用
    /// </summary>
    public class WwiseSegmentRef : WwiseNodeWithName
    {
        /// <summary>
        /// 初始化需要Music Segment的名称与GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseSegmentRef(string name, string id, WwiseParser parser) : base("SegmentRef", name, parser)
        {
            node.SetAttribute("ID", "{" + id + "}");
        }
    }
}
