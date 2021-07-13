using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;

namespace WwiseTools
{
    /// <summary>
    /// 对于Music Segment的引用
    /// </summary>
    public class WwiseSegmentRef : WwiseProperty
    {
        /// <summary>
        /// 初始化需要Music Segment的名称与GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseSegmentRef(string name, string id)
        {
            body = String.Format("<SegmentRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
