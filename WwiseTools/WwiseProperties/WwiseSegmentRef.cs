using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;

namespace WwiseTools
{
    public class WwiseSegmentRef : WwiseProperty
    {
        public WwiseSegmentRef(string name, string id)
        {
            body = String.Format("<SegmentRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
