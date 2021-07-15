using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    class WwiseMusicTrackSequence : WwiseUnit
    {
        public WwiseMusicTrackSequence(WwiseParser parser) : base("", "MusicTrackSequence", parser)
        {
        }

        protected override void AddChildrenList()
        {
            if (childrenList != null) return;
            AddChildNode(childrenList = new WwiseNode("ClipList", parser));
        }
    }
}
