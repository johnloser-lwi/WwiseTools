using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools
{
    /// <summary>
    /// Wwise中的随机容器(Random Container)
    /// </summary>
    public class WwiseRandomContainer : WwiseContainer
    {
        public WwiseRandomContainer(string name, WwiseParser parser) : base(name, "RandomSequenceContainer", parser)
        {
        }

        public WwiseRandomContainer(string name, string guid, WwiseParser parser) : base(name, "RandomSequenceContainer", guid, parser)
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
            AddProperty(new WwiseProperty("PlayMechanismStepOrContinuous", "int16", String.Format("{0}", s.ToString()), parser));
        }

        /// <summary>
        /// 设置是否为Shuffle
        /// </summary>
        /// <param name="shuffle"></param>
        public void SetShuffle(bool shuffle)
        {
            int s = 0;
            if (!shuffle) s = 1;
            AddProperty(new WwiseProperty("NormalOrShuffle", "int16", String.Format("{0}", s.ToString()), parser));
        }
    }
}
