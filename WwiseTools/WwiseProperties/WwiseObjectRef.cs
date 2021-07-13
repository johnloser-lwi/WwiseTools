using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    /// <summary>
    /// 对于Wwise物体(单元)的引用
    /// </summary>
    public class WwiseObjectRef : WwiseProperty
    {
        /// <summary>
        /// 初始化单元的名称、GUID以及所属工作单元的GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="work_unit_id"></param>
        public WwiseObjectRef(string name, string id, string work_unit_id)
        {
            body = String.Format("<ObjectRef Name=\"{0}\" ID=\"{{{1}}}\" WorkUnitID=\"{{{2}}}\"/>", name, id, work_unit_id);
        }
    }
}
