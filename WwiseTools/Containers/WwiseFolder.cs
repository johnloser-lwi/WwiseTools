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
        /// <param name="name"></param>
        public WwiseFolder(string name, WwiseParser parser) : base(name, "Folder", parser)
        {
        }

        /// <summary>
        /// 创建并设置名称、GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="guid"></param>
        /// <param name="parser"></param>
        public WwiseFolder(string name, string guid, WwiseParser parser) : base(name, "Folder", guid, parser)
        {
        }
    }
}
