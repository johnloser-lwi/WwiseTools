using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;
using WwiseTools.Basic;

namespace WwiseTools.Properties
{
    /// <summary>
    /// 对于Wwise物体(单元)的引用
    /// </summary>
    public class WwiseObjectRef : WwiseNodeWithName
    {
        /// <summary>
        /// 初始化单元的名称、GUID以及所属工作单元的GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="work_unit_id"></param>
        public WwiseObjectRef(string name, string id, string work_unit_id, WwiseParser parser) : base("ObjectRef", name, parser)
        {
            node.SetAttribute("ID", "{" + id + "}");
            node.SetAttribute("WorkUnitID", "{" + work_unit_id + "}");
        }
    }
}
