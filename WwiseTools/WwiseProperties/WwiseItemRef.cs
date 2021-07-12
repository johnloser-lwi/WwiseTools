using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    public class WwiseItemRef : WwiseProperty
    {
        public WwiseItemRef(string name, string id)
        {
            body = String.Format("<ItemRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
