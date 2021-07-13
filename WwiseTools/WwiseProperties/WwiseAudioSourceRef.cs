using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Properties
{
    /// <summary>
    /// 对于音频源(AudioSource)的引用
    /// </summary>
    public class WwiseAudioSourceRef : WwiseProperty
    {
        /// <summary>
        /// 初始化音频源名称以及GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public WwiseAudioSourceRef(string name, string id)
        {
            body = String.Format("<AudioSourceRef Name=\"{0}\" ID=\"{{{1}}}\"/>", name, id);
        }
    }
}
