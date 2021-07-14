using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Utils;

namespace WwiseTools
{
 
    /// <summary>
    /// Wwise中的Switch Container
    /// </summary>
    public class WwiseSwitchContainer : WwiseContainer
    {
        public WwiseSwitchContainer(string name, WwiseParser parser) : base(name, "SwitchContainer", parser)
        {
            AddChildrenList();
            AddChildNode(new WwiseNode("GroupingInfo", parser));
        }

        public WwiseSwitchContainer(string name, string guid, WwiseParser parser) : base(name, "SwitchContainer", guid, parser)
        {
            AddChildrenList();
            AddChildNode(new WwiseNode("GroupingInfo", parser));
        }
    }
}
