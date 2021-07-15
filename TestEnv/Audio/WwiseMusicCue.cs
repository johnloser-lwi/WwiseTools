using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WwiseTools.Basics;
using WwiseTools.Properties;
using WwiseTools.Utils;

namespace WwiseTools.Audio
{
    class WwiseMusicCue : WwiseUnit
    {
        public CueType Type => cueType;

        public enum CueType { Entry, Exit, Custom }
        private CueType cueType;

        public WwiseMusicCue(string name, CueType cueType, WwiseParser parser) : base(name, "MusicCue", parser)
        {
            this.cueType = cueType;
            AddProperty(new WwiseProperty("CueType", "int16", CueTypeChecker(cueType).ToString(), parser));
        }

        private int CueTypeChecker(CueType trackType)
        {
            switch (trackType)
            {
                case CueType.Entry:
                    return 0;
                case CueType.Exit:
                    return 1;
                case CueType.Custom:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
