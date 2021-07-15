using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Utils;

namespace WwiseTools.Reference
{
    /// <summary>
    /// 对于音频源(AudioSource)的引用
    /// </summary>
    public class WwiseAudioSourceRef : WwiseNodeWithName
    {
        /// <summary>
        /// 初始化音频源名称以及GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseAudioSourceRef(string name, string id, WwiseParser parser) : base("AudioSourceRef", name, parser)
        {
            node.SetAttribute("ID", "{" + id + "}");
        }
    }
}
