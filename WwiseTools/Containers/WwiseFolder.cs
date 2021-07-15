using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// WWise中的Virtual Folder
    /// </summary>
    public class WwiseFolder : WwiseUnit
    {
        /// <summary>
        /// 创建并设置名称
        /// </summary>
        /// <param name="_name"></param>
        public WwiseFolder(string _name, WwiseParser parser) : base(_name, "Folder", parser)
        {
        }

        /// <summary>
        /// 创建并设置名称、GUID
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="guid"></param>
        public WwiseFolder(string _name, string guid, WwiseParser parser) : base(_name, "Folder", guid, parser)
        {
        }
    }
}
