using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basic;

namespace WwiseTools.Audio
{
    class WwiseMusicTrackSequence : WwiseUnit
    {
        public WwiseMusicTrackSequence() : base("", "MusicTrackSequence")
        {
        }

        protected override void Init(string _name, string u_type, string audioSourceID)
        {
            this.unit_name = _name;
            this.guid = Guid.NewGuid().ToString().ToUpper().Trim();
            xml_head = String.Format("<{0} Name=\"{1}\" ID=\"{{{2}}}\">", u_type, unit_name, guid);
            xml_tail = String.Format("</{0}>", u_type);
        }

        protected override void AddChildrenList()
        {
            if (children != null) return;
            AddChildNode(children = new WwiseNode("ClipList"));
        }
    }
}
