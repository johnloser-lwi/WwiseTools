using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;

namespace WwiseTools
{
 
    /// <summary>
    /// Wwise中的Switch Container
    /// </summary>
    public class WwiseSwitchContainer : WwiseContainer
    {
        public WwiseSwitchContainer(string _name) : base(_name, "SwitchContainer")
        {
            Init(_name, "SwitchContainer", guid);
        }

        public WwiseSwitchContainer(string _name, string guid) : base(_name, "SwitchContainer", guid)
        {
            Init(_name, "SwitchContainer", guid);
        }

        protected override void Init(string _name, string u_type, string guid)
        {
            base.Init(_name, u_type, guid);
            AddChildrenList();
            AddChildNode(new WwiseNode("GroupingInfo"));
        }
    }
}
