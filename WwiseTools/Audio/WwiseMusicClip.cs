using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Properties;

namespace WwiseTools.Audio
{
    class WwiseMusicClip : WwiseContainer
    {
        public WwiseMusicClip(string _name, float endTrimOffset, string file, string audioSourceID) : base(_name, "MusicClip")
        {
            AddChildNode(new WwiseAudioSourceRef(name, audioSourceID));
            SetDefaultProperty(endTrimOffset);
        }

        private void SetDefaultProperty(float endTrimOffset)
        {
            AddProperty(new WwiseProperty("EndTrimOffset", "Real64", endTrimOffset.ToString()));
            AddProperty(new WwiseProperty("FadeInMode", "int16", "0"));
            AddProperty(new WwiseProperty("FadeOutMode", "int16", "0"));
        }

        protected override void Init(string _name, string u_type, string audioSourceID)
        {
            this.unit_name = _name;
            this.guid = Guid.NewGuid().ToString().ToUpper().Trim();
            xml_head = String.Format("<{0} Name=\"{1}\" ID=\"{{{2}}}\">", u_type, unit_name, guid);
            xml_tail = String.Format("</{0}>", u_type);
        }
    }
}
