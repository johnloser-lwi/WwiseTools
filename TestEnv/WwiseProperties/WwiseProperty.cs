using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Utils;

namespace WwiseTools.Properties
{
    /// <summary>
    /// Wwise属性，可添加至WwiseUnit
    /// </summary>
    public class WwiseProperty : Basic.WwiseNodeWithName// : IWwisePrintable
    {

        /// <summary>
        /// 初始化属性的名称、类型以及值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public WwiseProperty(string name, string type, string value, WwiseParser parser) : base ("Property", name, parser)
        {
            node.SetAttribute("Type", type);
            node.SetAttribute("Value", value);
        }
    }
}
