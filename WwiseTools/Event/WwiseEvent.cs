using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// Wwise中的事件
    /// </summary>
    public class WwiseEvent : WwiseUnit
    {
        public WwiseEvent(string _name, WwiseParser parser) : base(_name, "Event", parser)
        { 
        }
    }
}
