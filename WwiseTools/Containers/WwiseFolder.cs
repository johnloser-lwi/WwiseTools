using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public WwiseFolder(string _name) : base(_name, "Folder")
        {
        }

        /// <summary>
        /// 创建并设置名称、GUID
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="guid"></param>
        public WwiseFolder(string _name, string guid) : base(_name, "Folder", guid)
        {
        }
    }
}
