using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    /// <summary>
    /// 对于子单元的引用
    /// </summary>
    public class WwiseItemRef : WwiseProperty
    {
        /// <summary>
        /// 初始化子单元的名称以及GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseItemRef(string name, string id)
        {
            body = String.Format("<ItemRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
