using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;
using WwiseTools.Properties;

namespace WwiseTools
{
    /// <summary>
    /// Wwise中的随机容器(Random Container)
    /// </summary>
    public class WwiseRandomContainer : WwiseContainer
    {
        public WwiseRandomContainer(string _name) : base(_name, "RandomSequenceContainer")
        {
        }

        public WwiseRandomContainer(string _name, string guid) : base(_name, "RandomSequenceContainer", guid)
        {
        }

        /// <summary>
        /// 设置步进或者连续模式
        /// </summary>
        /// <param name="step"></param>
        public void SetStepOrContinous(bool step)
        {
            int s = 0;
            if (step) s = 1;
            AddProperty(new WwiseProperty("PlayMechanismStepOrContinuous", "int16", String.Format("{0}", s.ToString())));
        }

        /// <summary>
        /// 设置是否为Shuffle
        /// </summary>
        /// <param name="shuffle"></param>
        public void SetShuffle(bool shuffle)
        {
            int s = 0;
            if (!shuffle) s = 1;
            AddProperty(new WwiseProperty("NormalOrShuffle", "int16", String.Format("{0}", s.ToString())));
        }
    }
}
