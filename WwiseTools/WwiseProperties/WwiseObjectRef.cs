using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseObjectRef : WwiseProperty
    {
        public WwiseObjectRef(string name, string id, string work_unit_id)
        {
            body = String.Format("<ObjectRef Name=\"{0}\" ID=\"{{{1}}}\" WorkUnitID=\"{{{2}}}\"/>", name, id, work_unit_id);
        }
    }
}
