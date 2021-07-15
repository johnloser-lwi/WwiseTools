using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;
using WwiseTools.Reference;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    class WwiseMusicClip : WwiseContainer
    {
        public WwiseMusicClip(string name, float endTrimOffset, string file, string audioSourceID, WwiseParser parser) : base(name, "MusicClip", parser)
        {
            AddChildNode(new WwiseAudioSourceRef(name, audioSourceID, parser));
            SetDefaultProperty(endTrimOffset);
        }

        private void SetDefaultProperty(float endTrimOffset)
        {
            AddProperty(new WwiseProperty("EndTrimOffset", "Real64", endTrimOffset.ToString(), parser));
            AddProperty(new WwiseProperty("FadeInMode", "int16", "0", parser));
            AddProperty(new WwiseProperty("FadeOutMode", "int16", "0", parser));
        }
    }
}
